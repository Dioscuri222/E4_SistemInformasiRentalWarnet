using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Sistem_Warnet
{
    public partial class Setup : Form
    {
        public Setup()
        {
            InitializeComponent();
        }

        private void Setup_Load(object sender, EventArgs e)
        {
            UIHelper.ApplyTheme(this);
        }

        private void rbClient_CheckedChanged(object sender, EventArgs e)
        {
            // Nyalakan kolom IP hanya jika Client dipilih
            txtIPAddress.Enabled = rbClient.Checked;
            if (rbClient.Checked)
            {
                txtIPAddress.Focus();
            }
        }

        private void btnLanjut_Click(object sender, EventArgs e)
        {
            string configPath = AppDomain.CurrentDomain.BaseDirectory + "config.txt";
            string connectionString = "";

            if (rbServer.Checked)
            {
                // Format String untuk Server Lokal
                connectionString = "Data Source=.\\FASYALTP;Initial Catalog=DBWarnet;Integrated Security=True;Connection Timeout=5;";
            }
            else if (rbClient.Checked)
            {
                string ip = txtIPAddress.Text.Trim();
                if (string.IsNullOrEmpty(ip))
                {
                    MessageBox.Show("Masukkan IP Address Server terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Format String untuk Client (Menembak IP Server)
                connectionString = $"Data Source={ip}\\FASYALTP;Initial Catalog=DBWarnet;User ID=sa;Password=1138;Connection Timeout=5;";
            }

            try
            {
                // Buat dan tulis string tersebut ke dalam file config.txt
                File.WriteAllText(configPath, connectionString);

                // Kirim sinyal OK ke program utama lalu tutup pop-up
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menyimpan konfigurasi: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
