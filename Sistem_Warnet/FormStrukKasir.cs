using System;
using System.Data;
using System.Windows.Forms;

namespace Sistem_Warnet
{
    public partial class FormStrukKasir : Form
    {
        private string kodeVoucher;
        DAL dbLogic = new DAL();

        // Konstruktor menerima kode voucher dari Form Transaksi
        public FormStrukKasir(string kode)
        {
            InitializeComponent();
            this.kodeVoucher = kode;
        }

        private void FormStrukKasir_Load(object sender, EventArgs e)
        {
            try
            {
                // Tarik data dari database
                DataTable dt = dbLogic.CetakStrukKasir(kodeVoucher);

                // Load file desain Crystal Report (Pastikan Anda sudah mendesain ReportStruk.rpt)
                ReportStruk rpt = new ReportStruk();
                rpt.SetDataSource(dt);

                // Tampilkan ke viewer
                crystalReportViewer1.ReportSource = rpt;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat struk: " + ex.Message);
            }
        }
        private void crystalReportViewer1_load(object sender, EventArgs e)
        {
            // Tidak perlu kode di sini untuk saat ini
        }
    }
}