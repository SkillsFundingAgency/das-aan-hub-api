CREATE TABLE [dbo].[Permission]
(
	[Id] BIGINT NOT NULL IDENTITY(1, 1) PRIMARY KEY, 
    [PermissionName] NVARCHAR(200) NOT NULL, 
    [PermissionDescription] NVARCHAR(200) NOT NULL 
)
