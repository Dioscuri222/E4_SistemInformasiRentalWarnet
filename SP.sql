CREATE PROCEDURE sp_GetMasterPC
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        id_pc, 
        id_tier, 
        nomor_pc, 
        status
    FROM Master_PC
END
GO

CREATE PROCEDURE sp_InsertMasterPC
    @id_tier INT,
    @nomor_pc VARCHAR(10),
    @status VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Master_PC 
        (id_tier, nomor_pc, status)
    VALUES 
        (@id_tier, @nomor_pc, @status)
END
GO

CREATE PROCEDURE sp_GetMasterPCByID
    @id_pc INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        id_pc, 
        id_tier, 
        nomor_pc, 
        status
    FROM Master_PC
    WHERE id_pc = @id_pc
END
GO

CREATE PROCEDURE sp_UpdateMasterPC
    @id_pc INT,
    @id_tier INT,
    @nomor_pc VARCHAR(10),
    @status VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Master_PC
    SET 
        id_tier = @id_tier,
        nomor_pc = @nomor_pc,
        status = @status
    WHERE id_pc = @id_pc
END
GO

CREATE PROCEDURE sp_DeleteMasterPC
    @id_pc INT
AS
BEGIN
    SET NOCOUNT ON;
    
    DELETE FROM Master_PC
    WHERE id_pc = @id_pc
END
GO


CREATE PROCEDURE sp_CountMasterPC
    @Total INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT @Total = COUNT(*) FROM Master_PC
END
GO