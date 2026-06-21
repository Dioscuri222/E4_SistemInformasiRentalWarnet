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
                // 1. Tarik data dari database (menggunakan DAL)
                DataTable dt = dbLogic.CetakStrukKasir(kodeVoucher);

                // 2. Load file desain Crystal Report
                ReportStruk rpt = new ReportStruk();
                rpt.SetDataSource(dt);

                // 3. TAMBAHAN WAJIB: Injeksi parameter langsung ke file .rpt
                // Ini yang akan mencegah pop-up "Enter Parameter Values" muncul!
                rpt.SetParameterValue("@kode_voucher", kodeVoucher);

                // 4. Tampilkan ke viewer
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