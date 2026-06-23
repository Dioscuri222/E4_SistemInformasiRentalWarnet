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
            // Terapkan UI Helper jika Anda ingin warnanya senada
            UIHelper.FormatForm(this);
            LoadGrafikPendapatan();
        }

        private void LoadGrafikPendapatan()
        {
            try
            {
                DataTable dt = dbLogic.GetStatistikPendapatanTier();

                // 1. Bersihkan grafik bawaan (Dummy)
                chartPendapatan.Series.Clear();
                chartPendapatan.Titles.Clear();

                // 2. Buat Seri Grafik Baru
                Series series = new Series("Pendapatan");

                // Ubah menjadi SeriesChartType.Pie jika ingin bentuk lingkaran
                series.ChartType = SeriesChartType.Column;
                series.IsValueShownAsLabel = true; // Memunculkan nominal di atas batang
                series.LabelFormat = "N0"; // Format pemisah ribuan

                int totalKeseluruhan = 0;

                // 3. Masukkan data dari database ke dalam sumbu X dan Y grafik
                foreach (DataRow row in dt.Rows)
                {
                    string namaTier = row["nama_tier"].ToString();
                    int pendapatan = Convert.ToInt32(row["total_pendapatan"]);

                    // X = Nama Tier, Y = Pendapatan
                    series.Points.AddXY(namaTier, pendapatan);

                    // Kalkulasi total kumulatif
                    totalKeseluruhan += pendapatan;
                }

                // 4. Terapkan ke komponen layar
                chartPendapatan.Series.Add(series);
                chartPendapatan.Titles.Add("Statistik Pendapatan Berdasarkan Tier PC");

                lblTotalPendapatan.Text = "Total Pendapatan Keseluruhan: Rp " + totalKeseluruhan.ToString("N0");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat grafik: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {

        }
    }
}