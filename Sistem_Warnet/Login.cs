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
using static Sistem_Warnet.Warnet_Form;

namespace Sistem_Warnet
{
    public partial class Login_Form : Form
    {
        private DAL dbLogic = new DAL();

        public Login_Form()
        {
            InitializeComponent();
            txtPassword.UseSystemPasswordChar = true;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                // Memanggil fungsi login dari DAL
                Staff loggedInUser = dbLogic.CekLogin(txtUsername.Text, txtPassword.Text);

                if (loggedInUser != null)
                {
                    this.Hide();
                    if (loggedInUser.Role == "Admin")
                    {
                        new Warnet_Form().Show();
                    }
                    else
                    {
                        new Operator_Form(loggedInUser).Show();
                    }
                }
                else
                {
                    MessageBox.Show("Login Gagal! Username atau Password salah.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Koneksi gagal: " + ex.Message);
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            // Opsi ini bisa Anda hapus dari UI jika sudah tidak digunakan
            MessageBox.Show("Koneksi berhasil.");
        }

        private void Form1_Load(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
    }
}