CREATE TABLE dbo.ErrorLog
(
    Id              INT IDENTITY(1,1),
    ErrorDate       DATETIME CONSTRAINT [DF_ErrorLog_ErrorDate] DEFAULT (sysdatetime()) NOT NULL,
    LogLevel        NVARCHAR(128) NULL,
    [Location]      NVARCHAR(512) NULL,
    StackTrace      NVARCHAR(MAX) NULL,
    ProcedureName   NVARCHAR(128) NULL,
    ErrorNumber     INT NULL,
    ErrorSeverity   INT NULL,
    ErrorState      INT NULL,
    ErrorLine       INT NULL,
    ErrorMessage    NVARCHAR(4000) NULL,
    UserId          INT NULL,
    CONSTRAINT [PK_ErrorLog] PRIMARY KEY CLUSTERED ([Id] ASC),
)