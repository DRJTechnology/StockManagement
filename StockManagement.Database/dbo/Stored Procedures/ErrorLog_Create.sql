CREATE PROCEDURE [dbo].[ErrorLog_Create]
    @Success BIT OUTPUT,
    @Id INT OUTPUT,
    @ProcedureName NVARCHAR(128) = NULL,
    @ErrorNumber INT = NULL,
    @ErrorSeverity INT = NULL,
    @ErrorState INT = NULL,
    @ErrorLine INT = NULL,
    @ErrorMessage NVARCHAR(4000) = NULL,
    @UserId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @Err INT = 0;
    
    BEGIN TRY
        INSERT INTO dbo.ErrorLog
        (ErrorDate, ProcedureName, ErrorNumber, ErrorSeverity, ErrorState, ErrorLine, ErrorMessage, UserId)
        VALUES 
        (GETDATE(), @ProcedureName, @ErrorNumber, @ErrorSeverity, @ErrorState, @ErrorLine, @ErrorMessage, @UserId);
        
        SET @Id = SCOPE_IDENTITY();
        SET @Success = 1;
        SET @Err = 0;
    END TRY
    BEGIN CATCH
        SET @Success = 0;
        SET @Id = NULL;
        SET @Err = ERROR_NUMBER();
    END CATCH
    
    RETURN @Err;
END