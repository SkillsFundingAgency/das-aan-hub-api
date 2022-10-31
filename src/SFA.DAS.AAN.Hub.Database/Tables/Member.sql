CREATE TABLE [dbo].[Member]
(
	[Id] BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [UserType] NVARCHAR(10) NOT NULL, 
    [Status] NVARCHAR(10) NOT NULL, 
    [Information] NVARCHAR(MAX) NOT NULL, 
    [RegionId] INT NULL, 
    [Organisation] NVARCHAR(MAX) NULL, 
    [Joined] DATETIME NOT NULL, 
    [Created] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [Updated] DATETIME NULL, 
    [Deleted] DATETIME NULL 
)
