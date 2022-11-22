CREATE TABLE [dbo].[Documents]
(
    [Id] BIGINT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    [CreatedBy] UNIQUEIDENTIFIER NOT NULL, 
    [Created] DATETIME NOT NULL, 
    [Title] NVARCHAR(200) NOT NULL, 
    [FileName] NCHAR(200) NOT NULL, 
    [Keywords] NVARCHAR(MAX) NOT NULL, 
    [Regions] NVARCHAR(200) NOT NULL, 
    [Url] NVARCHAR(256) NOT NULL
    CONSTRAINT [FK_Documents_Member] FOREIGN KEY ([CreatedBy]) REFERENCES [Member]([Id])
)
