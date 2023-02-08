CREATE TABLE [dbo].[Profile]
(
	[Id] BIGINT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    [UserType] NVARCHAR(20) NOT NULL ,
    [Category] NVARCHAR(20) NOT NULL, 
    [Description] NVARCHAR(200) NOT NULL, 
    [Ordering] INT NOT NULL
)
