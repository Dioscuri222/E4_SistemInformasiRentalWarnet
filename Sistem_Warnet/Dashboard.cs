using System;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Sistem_Warnet
{
    public partial class Dashboard_Form : Form
    {
        private DAL dbLogic = new DAL();

        public Dashboard_Form()
        {
            InitializeComponent();
        }

        private void Dashboard_Form_Load(object sender, EventArgs e)
        {
           // UIHelper.FormatForm(this);

            // 1. Matikan sementara trigger event agar tidak error saat memasukkan data
            cmbFilter.SelectedIndexChanged -= cmbFilter_SelectedIndexChanged;

            // 2. Isi ComboBox dengan pilihan filter
            cmbFilter.Items.Clear();
            cmbFilter.Items.Add("Semua Waktu");
            cmbFilter.Items.Add("Hari Ini");
            cmbFilter.Items.Add("Minggu Ini");
            cmbFilter.Items.Add("Bulan Ini");

            // 3. Nyalakan lagi trigger event-nya
            cmbFilter.SelectedIndexChanged += cmbFilter_SelectedIndexChanged;

            // 4. Pilih default: "Semua Waktu" (Ini akan otomatis memanggil grafik pertama kali)
            cmbFilter.SelectedIndex = 0;
        }

        // EVENT: Saat pilihan dropdown diganti
        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Ambil teks yang sedang dipilih dan lempar ke fungsi load grafik
            string filterPilihan = cmbFilter.Text;
            LoadGrafikPendapatan(filterPilihan);
        }

        // Menerima parameter filter
        private void LoadGrafikPendapatan(string filterWaktu)
        {
            try
            {
                // Panggil DAL dengan mengirimkan filter
                DataTable dt = dbLogic.GetStatistikPendapatanTier(filterWaktu);

                chartPendapatan.Series.Clear();
                chartPendapatan.Titles.Clear();

                Series series = new Series("Pendapatan");
                series.ChartType = SeriesChartType.Column;
                series.IsValueShownAsLabel = true;
                series.LabelFormat = "N0";

                int totalKeseluruhan = 0;

                foreach (DataRow row in dt.Rows)
                {
                    string namaTier = row["nama_tier"].ToString();
                    int pendapatan = Convert.ToInt32(row["total_pendapatan"]);

                    series.Points.AddXY(namaTier, pendapatan);
                    totalKeseluruhan += pendapatan;
                }

                chartPendapatan.Series.Add(series);

                // Ubah judul agar sesuai dengan filter
                chartPendapatan.Titles.Add($"Statistik Pendapatan Berdasarkan Tier PC ({filterWaktu.ToUpper()})");

                lblTotalPendapatan.Text = $"Total Pendapatan {filterWaktu}: Rp " + totalKeseluruhan.ToString("N0");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat grafik: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}