using System;
using System.Collections.Generic;
using System.Configuration; // WAJIB DITAMBAHKAN
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Sistem_Warnet
{
    internal class DAL
    {
        // Mengambil string koneksi secara otomatis dari tag <connectionStrings> di App.config
        private string connectionString = ConfigurationManager.ConnectionStrings["KoneksiWarnet"].ConnectionString;

        // Konstruktor kosong karena DAL sekarang bersifat Stateless
        public DAL()
        {
        }

        // ==========================================
        // KUMPULAN FUNGSI CRUD UNTUK MASTER PC
        // ==========================================
        public DataTable GetAllMasterPC()
        {
            DataTable dt = new DataTable();
            using (SqlConnection localConn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM vw_DataPC", localConn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public DataTable GetTierComboBox()
        {
            DataTable dt = new DataTable();
            using (SqlConnection localConn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetTierComboBox", localConn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public DataTable SearchMasterPC(string keyword)
        {
            DataTable dt = new DataTable();
            using (SqlConnection localConn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_SearchMasterPC", localConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@search", "%" + keyword + "%");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public void InsertMasterPC(int idTier, string nomorPc, string status)
        {
            using (SqlConnection localConn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_InsertMasterPC", localConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tier", idTier);
                cmd.Parameters.AddWithValue("@nomor", nomorPc);
                cmd.Parameters.AddWithValue("@status", status);

                localConn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateMasterPC(int idTier, string nomorPc, string status, int idPc)
        {
            using (SqlConnection localConn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateMasterPC", localConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tier", idTier);
                cmd.Parameters.AddWithValue("@nomor", nomorPc);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@id", idPc);

                localConn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteMasterPC(int idPc)
        {
            using (SqlConnection localConn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteMasterPC", localConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", idPc);

                localConn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public int CountMasterPC()
        {
            using (SqlConnection localConn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_CountMasterPC_Output", localConn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter outputParam = new SqlParameter("@Total", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);

                localConn.Open();
                cmd.ExecuteNonQuery();

                return Convert.ToInt32(outputParam.Value);
            }
        }

        public void ResetMasterPC()
        {
            using (SqlConnection localConn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_ResetMasterPC", localConn);
                cmd.CommandType = CommandType.StoredProcedure;
                localConn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // ==========================================
        // FUNGSI UNTUK LOGIN & TRANSAKSI KASIR
        // ==========================================

        public Staff CekLogin(string username, string password)
        {
            using (SqlConnection localConn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_Login", localConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);

                localConn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Staff loggedInUser = new Staff();
                    loggedInUser.IdUser = Convert.ToInt32(reader["id_user"]);
                    loggedInUser.Username = reader["username"].ToString();
                    loggedInUser.Role = reader["role"].ToString();
                    return loggedInUser;
                }
                return null;
            }
        }

        public DataTable GetPCTersediaUntukTransaksi()
        {
            DataTable dt = new DataTable();
            using (SqlConnection localConn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetPCTersedia", localConn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public void ResetStatusPCSemuaTersedia()
        {
            using (SqlConnection localConn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Master_PC SET status = 'Tersedia'", localConn);
                localConn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void ProsesPembelianVoucher(int idUser, int idTier, int idPc, int durasiJam, int totalBayar, out string kodeVoucherAwal)
        {
            // 1. Generate Kode Voucher Acak (6 Karakter)
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            kodeVoucherAwal = new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());

            // 2. Eksekusi Stored Procedure Transaksi
            using (SqlConnection localConn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_ProsesTransaksiKasir", localConn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id_user", idUser);
                cmd.Parameters.AddWithValue("@id_tier", idTier);
                cmd.Parameters.AddWithValue("@id_pc", idPc);
                cmd.Parameters.AddWithValue("@durasi_jam", durasiJam);
                cmd.Parameters.AddWithValue("@total_bayar", totalBayar);
                cmd.Parameters.AddWithValue("@kode_voucher", kodeVoucherAwal);

                localConn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public DataTable GetStatistikPendapatanTier()
        {
            DataTable dt = new DataTable();
            using (SqlConnection localConn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_StatistikPendapatanTier", localConn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public DataTable CetakStrukKasir(string kodeVoucher)
        {
            DataTable dtStruk = new DataTable();
            // KUNCI UTAMA: Beri nama tabel agar dikenali oleh Crystal Reports
            dtStruk.TableName = "sp_CetakStruk;1";

            using (SqlConnection localConn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_CetakStruk", localConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@kode_voucher", kodeVoucher);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtStruk);
            }
            return dtStruk;
        }

        public int ImportDataPCMassal(DataTable dtExcel)
        {
            int jumlahBerhasil = 0;
            using (SqlConnection localConn = new SqlConnection(connectionString))
            {
                localConn.Open();

                foreach (DataRow row in dtExcel.Rows)
                {
                    try
                    {
                        string nomor = row["Nomor PC"].ToString().ToUpper().Trim();
                        string tier = row["Tier"].ToString().Trim();
                        string status = row["Status"].ToString().Trim();

                        if (string.IsNullOrEmpty(nomor)) continue;

                        int idTier = (tier.ToUpper() == "VIP") ? 2 : 1;

                        SqlCommand cmd = new SqlCommand("sp_InsertMasterPC", localConn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@tier", idTier);
                        cmd.Parameters.AddWithValue("@nomor", nomor);
                        cmd.Parameters.AddWithValue("@status", status);

                        cmd.ExecuteNonQuery();
                        jumlahBerhasil++;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            return jumlahBerhasil;
        }

        public DataRow LoginClient(string kodeVoucher)
        {
            DataTable dt = new DataTable();
            using (SqlConnection localConn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_LoginClient", localConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@kode", kodeVoucher);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        public void LogoutClient(string kodeVoucher)
        {
            using (SqlConnection localConn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_LogoutClient", localConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@kode", kodeVoucher);

                localConn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public DataTable GetRekapTransaksi(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            DataTable dt = new DataTable();
            using (SqlConnection localConn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetRekapTransaksi", localConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tglMulai", tanggalMulai.Date);
                cmd.Parameters.AddWithValue("@tglSelesai", tanggalSelesai.Date.AddDays(1).AddSeconds(-1));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }
    }
}