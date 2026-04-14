using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
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

                string query = "SELECT p.id_pc, p.nomor_pc, t.nama_tier, p.status " +
                               "FROM Master_PC p JOIN Tier_PC t ON p.id_tier = t.id_tier";

                SqlCommand cmd = new SqlCommand(query, conn);
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

                string query = "SELECT id_tier, nama_tier FROM Tier_PC";
                SqlCommand cmd = new SqlCommand(query, conn);
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

                string query = "SELECT * FROM Master_PC WHERE nomor_pc LIKE @search";
                SqlCommand cmd = new SqlCommand(query, conn);
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

            try
            {
                string query = "INSERT INTO Master_PC (id_tier, nomor_pc, status) VALUES (@tier, @nomor, @status)";
                SqlCommand cmd = new SqlCommand(query, conn);
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
                string query = "SELECT COUNT(*) FROM Master_PC";
                SqlCommand cmd = new SqlCommand(query, conn);

                if (conn.State == ConnectionState.Closed) conn.Open();
                int total = (int)cmd.ExecuteScalar();

                lblTotal.Text = "Total PC Terdaftar: " + total.ToString(); 
            } catch (Exception ex)
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
                    string query = "DELETE FROM Master_PC WHERE id_pc = @id";
                    SqlCommand cmd = new SqlCommand(query, conn);
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

                string query = "SELECT * FROM Master_PC";
                SqlCommand cmd = new SqlCommand(query, conn);

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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menampilkan data: " + ex.Message);
            }
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

            try
            {
                string id = dataGridView1.SelectedRows[0].Cells["id_pc"].Value.ToString();

                string query = "UPDATE Master_PC SET id_tier=@tier, nomor_pc=@nomor, status=@status WHERE id_pc=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
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
    }
}
