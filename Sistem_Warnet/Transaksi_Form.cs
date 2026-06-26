using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Sistem_Warnet
{
    public partial class Transaksi_Form : Form
    {
        private Staff currentOperator;
        private int totalBayar = 0;
        private int hargaPerJam = 0;
        private int idTierTerpilih = 0;

        private DAL dbLogic = new DAL();

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
            UIHelper.ApplyTheme(this);

            if (currentOperator != null)
            {
                lblOperator.Text = "Operator: " + currentOperator.Username;
            }

            txtKembalian.ReadOnly = true;
            cmbTier.Enabled = false;
            txtKembalian.Text = "Rp 0";
            lblTotalBayar.Text = "Rp 0";

            LoadDataPC();
        }

        private void LoadDataPC()
        {
            try
            {
                DataTable dt = dbLogic.GetPCTersediaUntukTransaksi();
                cmbNoPC.DataSource = dt;
                cmbNoPC.DisplayMember = "nomor_pc";
                cmbNoPC.ValueMember = "id_pc";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data PC: " + ex.Message);
            }
        }

        private void cmbNoPC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbNoPC.SelectedItem != null)
            {
                DataRowView row = cmbNoPC.SelectedItem as DataRowView;
                if (row != null)
                {
                    cmbTier.Text = row["nama_tier"].ToString();
                    hargaPerJam = Convert.ToInt32(row["harga_per_jam"]);
                    idTierTerpilih = Convert.ToInt32(row["id_tier"]);

                    HitungTotalBayar();
                }
            }
        }

        private void nudDurasiJam_ValueChanged(object sender, EventArgs e)
        {
            HitungTotalBayar();
        }

        private void HitungTotalBayar()
        {
            int durasiJam = Convert.ToInt32(nudDurasiJam.Value);
            lblMenit.Text = (durasiJam * 60).ToString() + " Menit";

            totalBayar = durasiJam * hargaPerJam;
            lblTotalBayar.Text = "Rp " + totalBayar.ToString("N0");

            HitungKembalian();
        }

        private void txtUangTunai_TextChanged(object sender, EventArgs e)
        {
            HitungKembalian();
        }

        private void HitungKembalian()
        {
            if (int.TryParse(txtUangTunai.Text, out int uangTunai))
            {
                int kembalian = uangTunai - totalBayar;
                txtKembalian.Text = kembalian < 0 ? "Uang Kurang!" : "Rp " + kembalian.ToString("N0");
            }
            else
            {
                txtKembalian.Text = "Rp 0";
            }
        }

        // --- REVISI: Penangkapan error dan pengiriman nominal uang tunai ---
        private void button2_Click(object sender, EventArgs e)
        {
            if (cmbNoPC.SelectedValue == null || totalBayar == 0) return;

            // Pastikan operator mengisi nominal uang tunai yang valid
            if (!int.TryParse(txtUangTunai.Text, out int uangTunaiBayar))
            {
                MessageBox.Show("Silakan masukkan nominal uang tunai yang valid terlebih dahulu.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int idPc = Convert.ToInt32(cmbNoPC.SelectedValue);
                int durasiJam = Convert.ToInt32(nudDurasiJam.Value);
                string kodeVoucherBaru;

                // Mengirimkan parameter uangTunaiBayar ke DAL
                dbLogic.ProsesPembelianVoucher(currentOperator.IdUser, idTierTerpilih, idPc, durasiJam, totalBayar, uangTunaiBayar, out kodeVoucherBaru);

                // Baris pemanggilan form struk ini hanya akan dieksekusi jika fungsi di atas sukses tanpa error
                FormStrukKasir formStruk = new FormStrukKasir(kodeVoucherBaru);
                formStruk.ShowDialog();

                txtUangTunai.Clear();
                nudDurasiJam.Value = 1;
                LoadDataPC();
            }
            catch (SqlException ex)
            {
                // Menangkap pesan RAISERROR ("TRANSAKSI DITOLAK: Uang tunai kurang!") dari database
                MessageBox.Show(ex.Message, "Transaksi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Sistem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnResertPCTest_Click(object sender, EventArgs e)
        {
            try
            {
                dbLogic.ResetStatusPCSemuaTersedia();
                MessageBox.Show("Berhasil! Semua PC sekarang statusnya kembali 'Tersedia'.", "Debugging");
                LoadDataPC();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal reset PC: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new Operator_Form(currentOperator).Show();
            this.Close();
        }

        private void lblWaktu_Click(object sender, EventArgs e) { }
        private void txtKembalian_TextChanged(object sender, EventArgs e) { }
        private void cmbTier_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}