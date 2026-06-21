using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistem_Warnet
{
    internal class DAL
    {
        private string connectionString = "Data Source=FASYALTP\\FASYALTP;Initial Catalog=DBWarnet;Integrated Security=True";
        private SqlConnection conn;

        // TAMBAHAN WAJIB: Konstruktor untuk menginisialisasi conn
        public DAL()
        {
            conn = new SqlConnection(connectionString);
        }

        public void ProsesPembelianVoucher(int idUser, int idTier, int idPc, int durasiJam, int totalBayar, out string kodeVoucherAwal)
        {
            // 1. Generate Kode Voucher Acak (6 Karakter)
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            kodeVoucherAwal = new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());

            try
            {
                // 2. Buka Koneksi
                if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

                // 3. Panggil Stored Procedure Transaksi yang baru
                SqlCommand cmd = new SqlCommand("sp_ProsesTransaksiKasir", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                // Lempar parameter ke dalam SP
                cmd.Parameters.AddWithValue("@id_user", idUser);
                cmd.Parameters.AddWithValue("@id_tier", idTier);
                cmd.Parameters.AddWithValue("@id_pc", idPc);
                cmd.Parameters.AddWithValue("@durasi_jam", durasiJam);
                cmd.Parameters.AddWithValue("@total_bayar", totalBayar);
                cmd.Parameters.AddWithValue("@kode_voucher", kodeVoucherAwal);

                // Eksekusi
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // Tangkap pesan error dari RAISERROR di SQL Server
                throw new Exception("Transaksi Gagal: " + ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open) conn.Close();
            }
        }

        public DataTable CetakStrukKasir(string kodeVoucher)
        {
            if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

            SqlCommand cmd = new SqlCommand("sp_CetakStruk", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@kode_voucher", kodeVoucher);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dtStruk = new DataTable();
            da.Fill(dtStruk);

            conn.Close();
            return dtStruk;
        }
    }
}