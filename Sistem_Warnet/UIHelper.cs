using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sistem_Warnet
{
    public static class UIHelper
    {
        // 1. Tema untuk Form Dasar
        public static void FormatForm(Form frm)
        {
            frm.BackColor = Color.FromArgb(240, 244, 248);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Font = new Font("Segoe UI", 10f, FontStyle.Regular); 
        }

        // 2. Tema untuk Tombol Utama (Simpan, Update)
        public static void FormatPrimaryButton(Button btn)
        {
            btn.BackColor = Color.FromArgb(41, 128, 185); 
            btn.ForeColor = Color.White; 
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            btn.Cursor = Cursors.Hand; 
        }

        // 3. Tema untuk Tombol Peringatan (Delete, Logout)
        public static void FormatDangerButton(Button btn)
        {
            btn.BackColor = Color.FromArgb(231, 76, 60); 
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
        }

        // 4. Tema untuk DataGridView
        public static void FormatGrid(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.EnableHeadersVisualStyles = false; 

            // Desain Header Tabel
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94); 
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            // Desain Baris (Zebra Cross / Selang-seling)
            dgv.RowsDefaultCellStyle.BackColor = Color.White;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(236, 240, 241);
            dgv.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219); 

            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; 
        }
    }
}
