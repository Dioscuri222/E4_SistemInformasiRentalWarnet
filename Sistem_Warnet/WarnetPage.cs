using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sistem_Warnet
{
    public partial class Warnet_Form : Form
    {
        private string connectionString = "Data Source=FASYALTP\\FASYALTP;Initial Catalog=DBWarnet;Integrated Security=True";
        private SqlConnection conn;
        public Warnet_Form()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

    private void LoadData()
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                // Refactoring pada LoadData 
                SqlCommand cmd = new SqlCommand("sp_LoadDataPC", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();

                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                dataGridView1.Columns.Add("id_pc", "ID PC");
                dataGridView1.Columns.Add("nomor_pc", "Nomor PC");
                dataGridView1.Columns.Add("nama_tier", "Tier");
                dataGridView1.Columns.Add("status", "Status");

                while (reader.Read())
                {
                    dataGridView1.Rows.Add(
                        reader["id_pc"].ToString(),
                        reader["nomor_pc"].ToString(),
                        reader["nama_tier"].ToString(),
                        reader["status"].ToString()
                    );
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal Load: " + ex.Message);
            }
        }

        private void LoadTierToComboBox()
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                // Refactoring pada LoadTierToComboBox
                SqlCommand cmd = new SqlCommand("sp_GetTierComboBox", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                cmbTier.DataSource = dt;
                cmbTier.DisplayMember = "nama_tier"; 
                cmbTier.ValueMember = "id_tier";     

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat Tier: " + ex.Message);
            }
        }

        private void Warnet_Form_Load(object sender, EventArgs e)
        {
            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("Tersedia");
            cmbStatus.Items.Add("Maintenance");

            LoadTierToComboBox();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }

                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();

                dataGridView1.Columns.Add("id_pc", "ID PC");
                dataGridView1.Columns.Add("nomor_pc", "Nomor PC");
                dataGridView1.Columns.Add("status", "Status");

                // Refactoring pada Search
                SqlCommand cmd = new SqlCommand("sp_SearchMasterPC", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@search", "%" + txtPencarian.Text + "%");

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    dataGridView1.Rows.Add(
                        reader["id_pc"].ToString(),
                        reader["nomor_pc"].ToString(),
                        reader["status"].ToString()
                    );
                }

                reader.Close();

                if (dataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("Data PC tidak ditemukan.");
                }
            }
            catch (Exception ex)
            {
             MessageBox.Show("Gagal mencari data: " + ex.Message);
            }
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (txtNoPC.Text == "")
            {
                MessageBox.Show("Nomor PC tidak boleh kosong!");
                return;
            }

            if (string.IsNullOrEmpty(txtNoPC.Text))
            {
                MessageBox.Show("Nomor PC harus diisi");
                return;
            }

            // Constraint Per PC
            string input = txtNoPC.Text.ToUpper();
            if (!(input.StartsWith("PC-") || input.StartsWith("PC-VIP-")))
            {
                MessageBox.Show("Format Nomor PC salah! Harus diawali dengan 'PC-' atau 'PC-VIP-'.\nContoh: PC-01 atau PC-VIP-01");
                txtNoPC.Focus();
                return;
            }

            try
            {
                // Refactoring Simpan
                SqlCommand cmd = new SqlCommand("sp_InsertMasterPC", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@tier", cmbTier.SelectedValue);
                cmd.Parameters.AddWithValue("@nomor", txtNoPC.Text);
                cmd.Parameters.AddWithValue("@status", cmbStatus.Text);

                if (conn.State == ConnectionState.Closed) conn.Open();
                cmd.ExecuteNonQuery();

                MessageBox.Show("Data berhasil disimpan!");
                LoadData();
            } catch (Exception ex)
            {
                MessageBox.Show("Gagal Simpan: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnTotal_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_CountMasterPC_Output", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter outputParam = new SqlParameter("@Total", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);

                cmd.ExecuteNonQuery();

                lblTotal.Text = "Total PC Terdaftar: " + outputParam.Value.ToString(); 
            } 
            catch (Exception ex)
            {
                MessageBox.Show("Gagal Hitung: " + ex.Message);
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih data yang ingin dihapus!");
                return;
            }

            DialogResult result = MessageBox.Show("Yakin ingin menghapus data?", "Konfirmasi", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                try
                {
                    string id = dataGridView1.SelectedRows[0].Cells["id_pc"].Value.ToString();

                    // Refactoring pada Delete
                    SqlCommand cmd = new SqlCommand("sp_DeleteMasterPC", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);

                    if (conn.State == ConnectionState.Closed) conn.Open();
                    cmd.ExecuteNonQuery();
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal Hapus: " + ex.Message);
                }
            }
        }

        private void dgvDataPC_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtNoPC.Text = row.Cells["nomor_pc"].Value.ToString();
                cmbStatus.Text = row.Cells["status"].Value.ToString();
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            // Memanggil ulang LoadData() agar tidak ada pengulangan kode yang panjang
            LoadData();
        }

        private void txtNoPC_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbTier_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih data di tabel yang ingin diubah!");
                return;
            }
            DialogResult result = MessageBox.Show("Yakin ingin mengupdate data?", "Konfirmasi", MessageBoxButtons.YesNo);
            if (result == DialogResult.No) return;

            try
            {
                string id = dataGridView1.SelectedRows[0].Cells["id_pc"].Value.ToString();

                // Refactoring pada Update
                SqlCommand cmd = new SqlCommand("sp_UpdateMasterPC", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@tier", cmbTier.SelectedValue);
                cmd.Parameters.AddWithValue("@nomor", txtNoPC.Text);
                cmd.Parameters.AddWithValue("@status", cmbStatus.Text);
                cmd.Parameters.AddWithValue("@id", id);

                if (conn.State == ConnectionState.Closed) conn.Open();
                cmd.ExecuteNonQuery();

                MessageBox.Show("Data berhasil diupdate!");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal Update: " + ex.Message);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("berhasil logout!");
            new Login_Form().Show();
                this.Close();
        }
        
    }
}
