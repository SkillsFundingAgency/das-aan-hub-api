CREATE TABLE [dbo].[Region]
(
	[Id] INT NOT NULL IDENTITY(1, 1) PRIMARY KEY, 
    [Area] NVARCHAR(50) NOT NULL, 
    [Ordering] INT NOT NULL 
)
