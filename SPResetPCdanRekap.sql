USE DBWarnet;
GO

-- =================================================================
-- SP BARU 1: Reset Master PC ke Backup (Dengan Proteksi Transaksi)
-- =================================================================
CREATE OR ALTER PROCEDURE sp_ResetMasterPC
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Validasi 1: Pastikan tabel backupnya nyata ada
        IF OBJECT_ID('dbo.Master_PC_Backup') IS NULL
        BEGIN
            RAISERROR('PROSES RESET GAGAL: Tabel Master_PC_Backup tidak ditemukan di database.', 16, 1);
            RETURN;
        END

        -- Validasi 2: Proteksi mutlak, dilarang reset jika ada biling kustomer yang menyala!
        IF EXISTS (SELECT 1 FROM Voucher_Sesi WITH (NOLOCK) WHERE status_sesi = 'Aktif')
        BEGIN
            RAISERROR('PROSES RESET DITOLAK: Masih ada kustomer yang sedang bermain! Matikan seluruh sesi billing terlebih dahulu.', 16, 1);
            RETURN;
        END

        DELETE FROM dbo.Master_PC;

        SET IDENTITY_INSERT dbo.Master_PC ON;
        INSERT INTO dbo.Master_PC (id_pc, id_tier, nomor_pc, status)
        SELECT id_pc, id_tier, nomor_pc, status FROM dbo.Master_PC_Backup;
        SET IDENTITY_INSERT dbo.Master_PC OFF;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        DECLARE @ErrMsg NVARCHAR(4000) = 'Rollback Reset Data PC: ' + ERROR_MESSAGE();
        RAISERROR(@ErrMsg, 16, 1);
    END CATCH
END;
GO


-- =================================================================
-- SP BARU 3: Get PC Tersedia Untuk Transaksi Kasir
-- =================================================================
CREATE OR ALTER PROCEDURE sp_GetPCTersedia
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.id_pc, p.nomor_pc, t.id_tier, t.nama_tier, t.harga_per_jam
    FROM Master_PC p WITH (NOLOCK)
    INNER JOIN Tier_PC t WITH (NOLOCK) ON p.id_tier = t.id_tier
    WHERE p.status = 'Tersedia';
END;
GO

-- =================================================================
-- SP BARU 4: Get Rekap Transaksi (Filter Tanggal)
-- =================================================================
CREATE OR ALTER PROCEDURE sp_GetRekapTransaksi
    @tglMulai DATETIME,
    @tglSelesai DATETIME
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        t.tgl_transaksi AS [Tanggal Transaksi],
        tr.nama_tier AS [Tier PC],
        t.durasi_jam AS [Durasi Jam],
        t.total_bayar AS [Total Bayar],
        u.username AS [Operator]
    FROM Transaksi_Pembelian t WITH (NOLOCK)
    INNER JOIN Tier_PC tr WITH (NOLOCK) ON t.id_tier = tr.id_tier
    INNER JOIN Pengguna_Staf u WITH (NOLOCK) ON t.id_user = u.id_user
    WHERE t.tgl_transaksi >= @tglMulai AND t.tgl_transaksi <= @tglSelesai
    ORDER BY t.tgl_transaksi DESC;
END;
GO