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
    public partial class Operator_Form : Form
    {
        private string connectionString = "Data Source=FASYALTP\\FASYALTP;Initial Catalog=DBWarnet;Integrated Security=True";
        private SqlConnection conn;
        private BindingSource bindingSource = new BindingSource();

        public Staff currentStaff;
        public Operator_Form(Staff user)
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
            this.currentStaff = user;
        }

        private void Staff_Form_Load(object sender, EventArgs e)
        {

            // === APLIKASIKAN TEMA UI ===
            UIHelper.FormatForm(this);
            UIHelper.FormatGrid(dataGridView1);

            // Tombol-tombol utama
            UIHelper.FormatPrimaryButton(btnSearch);

            // Tombol peringatan
            UIHelper.FormatDangerButton(btnLogout);
            // ============================

            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                string query = "SELECT * FROM vw_DataPC";
                SqlCommand cmd = new SqlCommand(query, conn);

                // Menerapkan Binding dan Disconnected Architecture
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                bindingSource.DataSource = dt;
                dataGridView1.DataSource = bindingSource;

                bindingNavigator1.BindingSource = bindingSource;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data: " + ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand cmd = new SqlCommand("sp_SearchMasterPC", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@search", "%" + txtPencarian.Text + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Pencarian Gagal: " + ex.Message);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("berhasil logout!");
            new Login_Form().Show();
            this.Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnPesan_Click(object sender, EventArgs e)
        {
            Transaksi_Form formTransaksi = new Transaksi_Form(this.currentStaff);
            formTransaksi.Show();
            this.Hide();
        }
    }
}
