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

        private void DisconnectDatabase()
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }

                MessageBox.Show("Disconnect Database Berhasil!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Disconnect Database Gagal: " + ex.Message);
            }

        }

        public Login_Form()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            ConnectDatabase();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {

        }
    }
}
