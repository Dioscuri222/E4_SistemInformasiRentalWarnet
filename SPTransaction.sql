CREATE PROCEDURE sp_ProsesTransaksiKasirLengkap
    @id_user INT,
    @id_tier INT,
    @id_pc INT,
    @durasi_jam INT,
    @total_bayar INT,
    @kode_voucher VARCHAR(20)
AS
BEGIN
    -- Matikan pesan ' (1 row affected)' agar performa jaringan jauh lebih cepat
    SET NOCOUNT ON;

    -- MULAI BLOK PENANGKAP ERROR (T-SQL TRY...CATCH)
    BEGIN TRY
        -- 1. KUNCI PINTU TRANSAKSI
        BEGIN TRANSACTION;

        -- Siapkan variabel penampung ID Transaksi yang baru lahir
        DECLARE @new_id_transaksi INT;

        -- [PROSES A] Insert Transaksi Pembelian
        INSERT INTO Transaksi_Pembelian (id_user, id_tier, tgl_transaksi, durasi_jam, total_bayar)
        VALUES (@id_user, @id_tier, GETDATE(), @durasi_jam, @total_bayar);

        -- SCOPE_IDENTITY() menangkap id_transaksi yang baru saja tercipta di baris atas
        SET @new_id_transaksi = SCOPE_IDENTITY();

        -- [PROSES B] Insert Voucher Sesi
        DECLARE @sisa_menit INT = @durasi_jam * 60;
        
        INSERT INTO Voucher_Sesi (kode_voucher, id_transaksi, id_pc, waktu_mulai, sisa_waktu_menit, status_sesi)
        VALUES (@kode_voucher, @new_id_transaksi, @id_pc, GETDATE(), @sisa_menit, 'Aktif');

        -- [PROSES C] Update Status PC
        UPDATE Master_PC 
        SET status = 'Digunakan' 
        WHERE id_pc = @id_pc;

        -- JIKA SAMPAI BARIS INI TIDAK ADA YANG MELEDAK, SAHKAN PERMANEN!
        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH
        -- JIKA ADA ERROR DI TITIK MANAPUN (Misal: kode voucher kembar / tipe data salah)
        -- CEK APAKAH TRANSAKSI MASIH TERBUKA
        IF @@TRANCOUNT > 0
        BEGIN
            -- BATALKAN SELURUH PERUBAHAN! Uang ditarik, PC kembali Tersedia.
            ROLLBACK TRANSACTION;
        END

        -- Tangkap pesan error asli dari SQL Server, lalu lempar ke aplikasi C#
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
GO