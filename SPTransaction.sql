-- 1. SP untuk Insert Transaksi (Mengembalikan ID baru dengan OUTPUT INSERTED)
CREATE PROCEDURE sp_InsertTransaksi
    @id_user INT,
    @id_tier INT,
    @durasi_jam INT,
    @total_bayar INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Transaksi_Pembelian (id_user, id_tier, durasi_jam, total_bayar) 
    OUTPUT INSERTED.id_transaksi 
    VALUES (@id_user, @id_tier, @durasi_jam, @total_bayar);
END
GO

-- 2. SP untuk Insert Voucher
CREATE PROCEDURE sp_InsertVoucher
    @kode_voucher VARCHAR(20),
    @id_transaksi INT,
    @id_pc INT,
    @sisa_waktu_menit INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Voucher_Sesi (kode_voucher, id_transaksi, id_pc, sisa_waktu_menit, status_sesi) 
    VALUES (@kode_voucher, @id_transaksi, @id_pc, @sisa_waktu_menit, 'Aktif');
END
GO

-- 3. SP untuk Update Status PC saat dibeli
CREATE PROCEDURE sp_UpdateStatusPCDigunakan
    @id_pc INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Master_PC SET status = 'Digunakan' WHERE id_pc = @id_pc;
END
GO

CREATE PROCEDURE sp_GetTabelPCKasir
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT p.id_pc AS 'ID PC', 
           p.nomor_pc AS 'Nomor PC', 
           t.nama_tier AS 'Kategori', 
           t.harga_per_jam AS 'Harga/Jam', 
           p.status AS 'Status'
    FROM Master_PC p 
    JOIN Tier_PC t ON p.id_tier = t.id_tier;
END
GO

CREATE PROCEDURE sp_GetLaporanPendapatan
    @TglAwal DATE,
    @TglAkhir DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT tp.id_transaksi AS 'ID Transaksi',
           t.nama_tier AS 'Kategori Paket',
           tp.durasi_jam AS 'Durasi (Jam)',
           tp.total_bayar AS 'Pendapatan (Rp)',
           tp.tgl_transaksi AS 'Waktu Pembelian'
    FROM Transaksi_Pembelian tp
    JOIN Tier_PC t ON tp.id_tier = t.id_tier
    WHERE CAST(tp.tgl_transaksi AS DATE) BETWEEN @TglAwal AND @TglAkhir;
END
GO