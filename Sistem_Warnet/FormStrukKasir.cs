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
                DataTable dt = dbLogic.CetakStrukKasir(kodeVoucher);

                ReportStruk rpt = new ReportStruk();
                rpt.SetDataSource(dt);

                // Trik ampuh: Gunakan angka 0 (indeks parameter pertama) 
                // daripada menggunakan nama string "@kode_voucher"
                rpt.SetParameterValue(0, kodeVoucher);

                crystalReportViewer1.ReportSource = rpt;

                // CATATAN PENTING: Jangan gunakan crystalReportViewer1.Refresh(); di sini!
                // Memanggil Refresh akan memicu Crystal Reports bertanya parameter lagi.
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