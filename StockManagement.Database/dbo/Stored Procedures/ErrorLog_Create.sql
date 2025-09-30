CREATE PROCEDURE [dbo].[ErrorLog_Create]
    @Success BIT OUTPUT,
    @Id INT OUTPUT,
    @LogLevel NVARCHAR(128) = NULL,
    @Location NVARCHAR(512) = NULL,
    @ErrorMessage NVARCHAR(4000) = NULL,
    @StackTrace NVARCHAR(MAX) = NULL,
    @UserId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @Err INT = 0;
    
    BEGIN TRY
        INSERT INTO dbo.ErrorLog
        (ErrorDate, LogLevel, [Location], ErrorMessage, StackTrace, UserId)
        VALUES 
        (GETDATE(), @LogLevel, @Location, @ErrorMessage, @StackTrace, @UserId);
        
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