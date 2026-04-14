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

                string query = "SELECT p.id_pc, p.nomor_pc, t.nama_tier, p.status" +
                                "FROM Master_PC p JOIN Tier_PC t ON p.id_tier = t.id_tier";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(reader);
                dgvDataPC.DataSource = dt;
                reader.Close();
            } catch (Exception ex)
            {
                MessageBox.Show("Gagal Load: " + ex.Message);
            }
        }

        private void Warnet_Form_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

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
    }
}
