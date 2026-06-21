using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sistem_Warnet
{
    public partial class Transaksi_Form : Form
    {
        // Variabel untuk menyimpan data operator dari form sebelumnya
        private Staff currentOperator;

        // Variabel untuk menyimpan total harga asli (angka murni) untuk perhitungan
        private int totalBayar = 0;

        // Konstruktor diubah agar wajib menerima objek Staff
        public Transaksi_Form(Staff kasir)
        {
            InitializeComponent();
            this.currentOperator = kasir;
        }
        public Transaksi_Form()
        {
            InitializeComponent();
        }

        private void Transaksi_Form_Load(object sender, EventArgs e)
        {
            // 1. Tampilkan nama operator yang sedang login ke label "Operator:"
            // Pastikan Anda memiliki Label di samping tulisan Operator dengan nama 'lblNamaOperator'
            lblOperator.Text = currentOperator.Username;

            // 2. Kunci kotak teks kembalian agar tidak bisa diketik manual
            txtKembalian.ReadOnly = true;

            // 3. Set nilai awal tampilan
            txtUangKembalian.Text = "Rp 0";
            lblTotalBayar.Text = "Rp 0";
        }

        private void lblWaktu_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Operator_Form opForm = new Operator_Form(currentOperator);
            opForm.Show();
            this.Close();
        }

        private void nudDurasiJam_ValueChanged(object sender, EventArgs e)
        {
            int durasiJam = Convert.ToInt32(nudDurasiJam.Value);

            // Konversi ke Menit
            int durasiMenit = durasiJam * 60;
            lblMenit.Text = durasiMenit.ToString() + " Menit";

            // Asumsi sementara harga per jam. Nanti logika aslinya diambil dari database
            // sesuai Tier PC yang dipilih di cmbTier
            int hargaPerJam = 5000;

            // Hitung Total Bayar
            totalBayar = durasiJam * hargaPerJam;
            lblTotalBayar.Text = "Rp " + totalBayar.ToString("N0");

            // Panggil ulang perhitungan kembalian jaga-jaga jika uang tunai sudah terisi duluan
            HitungKembalian();
        }

        private void txtUangTunai_TextChanged(object sender, EventArgs e)
        {
            HitungKembalian();
        }

        // Fungsi khusus untuk menghitung kembalian secara otomatis
        private void HitungKembalian()
        {
            // Cek apakah yang diketik benar-benar angka (mencegah error jika diketik huruf)
            if (int.TryParse(txtUangTunai.Text, out int uangTunai))
            {
                int kembalian = uangTunai - totalBayar;

                // Validasi jika uang pelanggan kurang
                if (kembalian < 0)
                {
                    txtUangKembalian.Text = "Uang Kurang!";
                }
                else
                {
                    // Menampilkan format uang (contoh: Rp 15.000)
                    txtUangKembalian.Text = "Rp " + kembalian.ToString("N0");
                }
            }
            else
            {
                // Jika textbox uang tunai kosong atau berisi karakter aneh
                txtUangKembalian.Text = "Rp 0";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Logika SQL Insert Transaksi (Anggota 3 dan Anggota 1 bisa kolaborasi di sini)
        }
    }
}
