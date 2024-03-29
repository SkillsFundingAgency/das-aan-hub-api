﻿CREATE TABLE [dbo].[Document]
(
    [Id] BIGINT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    [MemberId] UNIQUEIDENTIFIER NOT NULL, 
    [Title] NVARCHAR(200) NOT NULL, 
    [FileName] NCHAR(200) NOT NULL, 
    [Keywords] NVARCHAR(MAX) NOT NULL, 
    [Regions] NVARCHAR(200) NOT NULL, 
    [Url] NVARCHAR(256) NOT NULL
    CONSTRAINT [FK_Documents_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id])
)