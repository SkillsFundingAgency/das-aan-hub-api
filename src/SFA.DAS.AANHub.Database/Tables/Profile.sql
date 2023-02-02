CREATE TABLE [dbo].[Profile]
(
	[Id] BIGINT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    [ProfileName] NVARCHAR(200) NOT NULL, 
    [ProfileDescription] NVARCHAR(MAX) NOT NULL, 
    [Ordering] TINYINT NOT NULL
)
