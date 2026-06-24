USE DBWarnet;
GO

-- =================================================================
-- 1. SP INSERT MASTER PC (Pencegah Duplikasi & Cek Tier)
-- =================================================================
CREATE OR ALTER PROCEDURE sp_InsertMasterPC
    @tier INT,
    @nomor VARCHAR(10),
    @status VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Validasi Awal 1: Pastikan ID Tier yang diinput operator itu nyata
        IF NOT EXISTS (SELECT 1 FROM Tier_PC WHERE id_tier = @tier)
        BEGIN
            RAISERROR('GAGAL MENYIMPAN: ID Tier PC (%d) tidak ditemukan di Master Tier.', 16, 1, @tier);
            RETURN;
        END

        -- Validasi Awal 2: Cek apakah Nomor PC sudah dipakai komputer lain
        IF EXISTS (SELECT 1 FROM Master_PC WHERE nomor_pc = @nomor)
        BEGIN
            RAISERROR('GAGAL MENYIMPAN: Nomor PC "%s" sudah terdaftar di dalam sistem.', 16, 1, @nomor);
            RETURN;
        END

        INSERT INTO Master_PC (id_tier, nomor_pc, status) 
        VALUES (@tier, @nomor, @status);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;

        -- Tangkap error sistem (misal melanggar pagar CONSTRAINT CHK_NomorPC_Format)
        DECLARE @CustomErrMsg NVARCHAR(4000) = 'Rollback Simpan PC: ' + ERROR_MESSAGE();
        RAISERROR(@CustomErrMsg, 16, 1);
    END CATCH
END;
GO

-- =================================================================
-- 2. SP UPDATE MASTER PC (Pencegah Bentrok Nama PC)
-- =================================================================
CREATE OR ALTER PROCEDURE sp_UpdateMasterPC
    @tier INT,
    @nomor VARCHAR(10),
    @status VARCHAR(20),
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Validasi Awal 1: Pastikan PC yang mau diedit tidak gaib
        IF NOT EXISTS (SELECT 1 FROM Master_PC WHERE id_pc = @id)
        BEGIN
            RAISERROR('GAGAL UPDATE: Komputer dengan ID %d tidak ditemukan di database.', 16, 1, @id);
            RETURN;
        END

        -- Validasi Awal 2: Pastikan ganti nama PC tidak menabrak milik PC lain
        IF EXISTS (SELECT 1 FROM Master_PC WHERE nomor_pc = @nomor AND id_pc <> @id)
        BEGIN
            RAISERROR('GAGAL UPDATE: Nama "%s" sudah digunakan oleh komputer lain.', 16, 1, @nomor);
            RETURN;
        END

        UPDATE Master_PC 
        SET id_tier = @tier, nomor_pc = @nomor, status = @status 
        WHERE id_pc = @id;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;

        DECLARE @CustomErrMsg NVARCHAR(4000) = 'Rollback Update PC: ' + ERROR_MESSAGE();
        RAISERROR(@CustomErrMsg, 16, 1);
    END CATCH
END;
GO

-- =================================================================
-- 3. SP DELETE MASTER PC (Proteksi Relasi Voucher Aktif)
-- =================================================================
CREATE OR ALTER PROCEDURE sp_DeleteMasterPC
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM Master_PC WHERE id_pc = @id)
        BEGIN
            RAISERROR('GAGAL HAPUS: Komputer dengan ID tersebut sudah tidak ada.', 16, 1);
            RETURN;
        END

        -- Validasi Super Ketat: Jangan izinkan hapus PC jika kustomer sedang main di bilik itu!
        IF EXISTS (SELECT 1 FROM Voucher_Sesi WHERE id_pc = @id AND status_sesi = 'Aktif')
        BEGIN
            RAISERROR('GAGAL HAPUS: Komputer ini sedang digunakan oleh kustomer aktif! Selesaikan sesi billing terlebih dahulu.', 16, 1);
            RETURN;
        END

        DELETE FROM Master_PC WHERE id_pc = @id;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;

        -- Jika gagal karena terikat Foreign Key riwayat struk lama
        DECLARE @CustomErrMsg NVARCHAR(4000) = 'Rollback Hapus PC: ' + ERROR_MESSAGE();
        RAISERROR(@CustomErrMsg, 16, 1);
    END CATCH
END;
GO

-- =================================================================
-- 4. SP LOGOUT KUSTOMER (Menjamin Sesi Mati + Bilik Kosong Serentak)
-- =================================================================
CREATE OR ALTER PROCEDURE sp_LogoutClient
    @kode VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM Voucher_Sesi WHERE kode_voucher = @kode AND status_sesi = 'Aktif')
        BEGIN
            RAISERROR('LOGOUT DITOLAK: Kode voucher "%s" tidak valid atau sesinya sudah ditutup sebelumnya.', 16, 1, @kode);
            RETURN;
        END

        -- A. Matikan sesi billing
        UPDATE Voucher_Sesi 
        SET status_sesi = 'Selesai' 
        WHERE kode_voucher = @kode;
        
        -- B. Kembalikan status PC ke "Tersedia"
        UPDATE p
        SET p.status = 'Tersedia'
        FROM Master_PC p
        JOIN Voucher_Sesi v ON p.id_pc = v.id_pc
        WHERE v.kode_voucher = @kode;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;

        DECLARE @CustomErrMsg NVARCHAR(4000) = 'Rollback Logout Kustomer (Sesi & PC dikembalikan ke kondisi semula): ' + ERROR_MESSAGE();
        RAISERROR(@CustomErrMsg, 16, 1);
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE sp_ProsesTransaksiKasir
    @id_user INT,
    @id_tier INT,
    @id_pc INT,
    @durasi_jam INT,
    @total_bayar INT,
    @kode_voucher VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Validasi 1: Kunci baris PC ini sejenak (UPDLOCK), cek apakah statusnya benar-benar "Tersedia"
        DECLARE @statusPC VARCHAR(20);
        SELECT @statusPC = status FROM Master_PC WITH (UPDLOCK) WHERE id_pc = @id_pc;

        IF (@statusPC IS NULL)
        BEGIN
            RAISERROR('TRANSAKSI BATAL: Bilik PC yang dipilih tidak terdaftar di sistem.', 16, 1);
            RETURN;
        END

        IF (@statusPC <> 'Tersedia')
        BEGIN
            RAISERROR('TRANSAKSI BATAL: Komputer tersebut saat ini berstatus "%s". Silakan pilih PC lain.', 16, 1, @statusPC);
            RETURN;
        END

        -- Validasi 2: Jangan sampai kode voucher hasil random C# kembar dengan yang sudah ada
        IF EXISTS (SELECT 1 FROM Voucher_Sesi WHERE kode_voucher = @kode_voucher)
        BEGIN
            RAISERROR('TRANSAKSI BATAL: Terjadi bentrok kode voucher acak ("%s"). Silakan klik proses sekali lagi.', 16, 1, @kode_voucher);
            RETURN;
        END

        -- [PROSES A] Catat Pendapatan
        DECLARE @new_id_transaksi INT;
        INSERT INTO Transaksi_Pembelian (id_user, id_tier, tgl_transaksi, durasi_jam, total_bayar)
        VALUES (@id_user, @id_tier, GETDATE(), @durasi_jam, @total_bayar);

        SET @new_id_transaksi = SCOPE_IDENTITY();

        -- [PROSES B] Terbitkan Voucher
        DECLARE @sisa_menit INT = @durasi_jam * 60;
        INSERT INTO Voucher_Sesi (kode_voucher, id_transaksi, id_pc, waktu_mulai, sisa_waktu_menit, status_sesi)
        VALUES (@kode_voucher, @new_id_transaksi, @id_pc, GETDATE(), @sisa_menit, 'Aktif');

        -- [PROSES C] Kunci Bilik PC
        UPDATE Master_PC 
        SET status = 'Digunakan' 
        WHERE id_pc = @id_pc;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;

        DECLARE @CustomErrMsg NVARCHAR(4000) = 'Rollback Transaksi Kasir (Uang & Billing Dibatalkan): ' + ERROR_MESSAGE();
        RAISERROR(@CustomErrMsg, 16, 1);
    END CATCH
END;
GO