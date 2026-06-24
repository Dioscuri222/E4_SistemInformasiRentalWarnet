using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Sistem_Warnet
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Tentukan lokasi file config
            string configPath = AppDomain.CurrentDomain.BaseDirectory + "config.txt";

            // Jika file config.txt BELUM ADA, munculkan Form Setup
            if (!File.Exists(configPath))
            {
                Setup setup = new Setup();

                // Tahan aplikasi sampai pengguna menekan "Simpan & Lanjut" (DialogResult.OK)
                if (setup.ShowDialog() != DialogResult.OK)
                {
                    // Jika pengguna malah menekan tombol silang (X) pada pop-up, matikan seluruh aplikasi
                    return;
                }
            }

            // Jika config.txt sudah ada (atau baru saja dibuat oleh FormSetup), lanjut ke Login
            Application.Run(new Login_Form());
        }
    }
}