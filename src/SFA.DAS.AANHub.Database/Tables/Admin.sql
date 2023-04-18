﻿CREATE TABLE [dbo].[Admin]
(
    [MemberId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Email] NVARCHAR(256) NOT NULL, 
    [UserName] NVARCHAR(200) NOT NULL, 
    [Name] NVARCHAR(200) NOT NULL, 
    [LastUpdated] DATETIME NOT NULL
    CONSTRAINT [FK_Admin_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id])
)
