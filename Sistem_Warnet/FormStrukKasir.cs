using System;
using System.Data;
using System.Windows.Forms;

namespace Sistem_Warnet
{
    public partial class FormStrukKasir : Form
    {
        private string kodeVoucher;
        DAL dbLogic = new DAL();

        // Komponen WebBrowser untuk memproses cetakan HTML
        private WebBrowser wbCetak = new WebBrowser();

        public FormStrukKasir(string kode)
        {
            InitializeComponent();
            this.kodeVoucher = kode;

            // Daftarkan event setelah HTML selesai dimuat di memori
            wbCetak.DocumentCompleted += WbCetak_DocumentCompleted;
        }

        private void FormStrukKasir_Load(object sender, EventArgs e)
        {
            try
            {
                // 1. Tarik data transaksi dari SQL Server
                DataTable dt = dbLogic.CetakStrukKasir(kodeVoucher);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Data transaksi gagal diambil!");
                    return;
                }

                DataRow row = dt.Rows[0];

                // 2. Rancang Desain Struk Menggunakan HTML + CSS murni
                string desainHtml = $@"
                <html>
                <head>
                    <style>
                        body {{ 
                            font-family: 'Courier New', Courier, monospace; 
                            width: 280px; 
                            margin: 0; 
                            padding: 10px; 
                            font-size: 12px;
                        }}
                        .text-center {{ text-align: center; }}
                        .text-right {{ text-align: right; }}
                        .garis-putus-putus {{ border-top: 1px dashed #000; margin: 8px 0; }}
                        .kode-box {{ 
                            border: 2px dashed #000; 
                            padding: 10px; 
                            font-size: 18px; 
                            font-weight: bold; 
                            margin: 10px 0;
                        }}
                        .tabel-data {{ width: 100%; font-size: 12px; }}
                    </style>
                </head>
                <body>
                    <div class='text-center'>
                        <h3 style='margin:0;'>WARNET MENDUNIA</h3>
                        <p style='margin:0; font-size:10px;'>Jl. Jendral Sudirman No. 45</p>
                        <p style='font-size:10px;'>{Convert.ToDateTime(row["tgl_transaksi"]):dd-MM-yyyy HH:mm:ss}</p>
                    </div>

                    <div class='garis-putus-putus'></div>

                    <table class='tabel-data'>
                        <tr><td>Operator</td><td>: {row["nama_operator"]}</td></tr>
                        <tr><td>No. PC</td><td>: {row["nomor_pc"]}</td></tr>
                        <tr><td>Paket/Tier</td><td>: {row["nama_tier"]}</td></tr>
                        <tr><td>Durasi</td><td>: {row["durasi_jam"]} Jam</td></tr>
                    </table>

                    <div class='garis-putus-putus'></div>

                    <div class='text-center'>
                        <span>KODE VOUCHER LOGIN:</span>
                        <div class='kode-box'>{row["kode_voucher"]}</div>
                    </div>

                    <div class='garis-putus-putus'></div>

                    <table class='tabel-data' style='font-weight:bold;'>
                        <tr>
                            <td>TOTAL BAYAR</td>
                            <td class='text-right'>Rp {Convert.ToInt32(row["total_bayar"]):N0}</td>
                        </tr>
                    </table>

                    <div class='garis-putus-putus'></div>
                    <p class='text-center' style='font-size:10px; margin:0;'>Terima Kasih atas Kunjungan Anda</p>
                </body>
                </html>";

                // 3. Masukkan string HTML ke dalam engine WebBrowser
                wbCetak.DocumentText = desainHtml;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memproses struk HTML: " + ex.Message);
            }
        }

        private void WbCetak_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            // 4. KUNCI UTAMA: Jalankan perintah cetak otomatis setelah HTML selesai dirender
            // Gunakan wbCetak.ShowPrintDialog() jika ingin memunculkan pilihan printer
            // Gunakan wbCetak.Print() jika ingin langsung mencetak ke printer default tanpa pop-up
            wbCetak.ShowPrintDialog();

            // Tutup form struk otomatis setelah dialog cetak selesai ditangani operator
            this.Close();
        }
    }
}