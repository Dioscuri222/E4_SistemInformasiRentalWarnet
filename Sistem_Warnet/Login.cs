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
    public partial class Login_Form : Form
    {
        private readonly SqlConnection conn;
        private readonly string connectionString =
            "Data Source=FASYALTP\\FASYALTP;Initial Catalog=DBWarnet;Integrated Security=True";

        private void ConnectDatabase()
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)

                {
                    conn.Open();
                }

                MessageBox.Show("Koneksi berhasil!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Koneksi gagal: " + ex.Message);
            }

        }


        public Login_Form()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);

            //txtPassword.UseSystemPasswordChar = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("sp_Login", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@username", txtUsername.Text);
            cmd.Parameters.AddWithValue("@password", txtPassword.Text);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                Staff user = new Staff();
                user.Username = reader["username"].ToString();
                user.Role = reader["role"].ToString();

                reader.Close();
                this.Hide();

                if (user.Role == "Admin")
                {
                    new Warnet_Form().Show();
                }
                else
                {
                    new Staff_Form().Show();
                }
            }
            else
            {
                MessageBox.Show("Login Gagal!");
            }
            reader.Close();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            ConnectDatabase();
        }


    }
}
