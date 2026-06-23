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
        private int idTierTerpilih = 0; // Tambahan: Menyimpan ID Tier dari PC yang dipilih

        DAL dbLogic = new DAL(); // Panggil class DAL

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
            // Tampilkan nama operator
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
                // Deklarasikan connection string dan SqlConnection secara lokal di sini
                string connString = "Data Source=FASYALTP\\FASYALTP;Initial Catalog=DBWarnet;Integrated Security=True";

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string query = @"
                        SELECT p.id_pc, p.nomor_pc, t.id_tier, t.nama_tier, t.harga_per_jam
                        FROM Master_PC p
                        INNER JOIN Tier_PC t ON p.id_tier = t.id_tier
                        WHERE p.status = 'Tersedia'";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbNoPC.DataSource = dt;
                    cmbNoPC.DisplayMember = "nomor_pc";
                    cmbNoPC.ValueMember = "id_pc";
                }
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
                    idTierTerpilih = Convert.ToInt32(row["id_tier"]); // Simpan ID Tier

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

            int durasiMenit = durasiJam * 60;
            lblMenit.Text = durasiMenit.ToString() + " Menit";

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

                if (kembalian < 0)
                {
                    txtKembalian.Text = "Uang Kurang!";
                }
                else
                {
                    txtKembalian.Text = "Rp " + kembalian.ToString("N0");
                }
            }
            else
            {
                txtKembalian.Text = "Rp 0";
            }
        }

        // TOMBOL BATAL / KEMBALI
        private void button3_Click(object sender, EventArgs e)
        {
            Operator_Form opForm = new Operator_Form(currentOperator);
            opForm.Show();
            this.Close();
        }

        // TOMBOL CETAK PEMBAYARAN (PROSES TRANSAKSI)
        private void button2_Click(object sender, EventArgs e)
        {
            // Validasi Input
            if (cmbNoPC.SelectedValue == null || totalBayar == 0) return;

            try
            {
                int idPc = Convert.ToInt32(cmbNoPC.SelectedValue);
                int durasiJam = Convert.ToInt32(nudDurasiJam.Value);
                string kodeVoucherBaru;

                // 1. Simpan Transaksi ke Database
                dbLogic.ProsesPembelianVoucher(currentOperator.IdUser, idTierTerpilih, idPc, durasiJam, totalBayar, out kodeVoucherBaru);

                // 2. KUNCI UTAMA: Buka Form Crystal Reports (Bukan cetak HTML)
                FormStrukKasir formStruk = new FormStrukKasir(kodeVoucherBaru);
                formStruk.ShowDialog();

                // 3. Reset form setelah jendela Crystal Reports ditutup oleh kasir
                txtUangTunai.Clear();
                nudDurasiJam.Value = 1;
                LoadDataPC();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblWaktu_Click(object sender, EventArgs e) { }
        private void txtKembalian_TextChanged(object sender, EventArgs e) { }
        private void cmbTier_SelectedIndexChanged(object sender, EventArgs e) { }

        private void btnResertPCTest_Click(object sender, EventArgs e)
        {
            try
            {
                string connString = "Data Source=FASYALTP\\FASYALTP;Initial Catalog=DBWarnet;Integrated Security=True";

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    // Eksekusi query paksa untuk mereset seluruh PC
                    SqlCommand cmd = new SqlCommand("UPDATE Master_PC SET status = 'Tersedia'", conn);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Berhasil! Semua PC sekarang statusnya kembali 'Tersedia'.", "Debugging");

                // Panggil ulang LoadDataPC agar ComboBox langsung terisi lagi
                LoadDataPC();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal reset PC: " + ex.Message);
            }
        }
    }
}