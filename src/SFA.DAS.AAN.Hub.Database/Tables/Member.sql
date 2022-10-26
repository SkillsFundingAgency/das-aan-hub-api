CREATE TABLE [dbo].[Member]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [UserType] NVARCHAR(MAX) NOT NULL, 
    [Status] NVARCHAR(MAX) NOT NULL, 
    [Information] NVARCHAR(MAX) NOT NULL, 
    [RegionId] INT NULL, 
    [Organisation] NVARCHAR(MAX) NULL, 
    [Joined] DATETIME NOT NULL, 
    [Created] DATETIME NOT NULL, 
    [Updated] DATETIME NOT NULL, 
    [Deleted] DATETIME NOT NULL 
)
