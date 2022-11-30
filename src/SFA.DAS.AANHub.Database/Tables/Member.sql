﻿CREATE TABLE [dbo].[Member]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
    [UserType] NVARCHAR(10) NOT NULL, 
    [Status] NVARCHAR(10) NOT NULL, 
    [Information] NVARCHAR(MAX) NULL, 
    [RegionId] INT NULL, 
    [Organisation] NVARCHAR(MAX) NULL, 
    [Joined] DATETIME NOT NULL, 
    [Created] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [Updated] DATETIME NULL, 
    [Deleted] DATETIME NULL 
    CONSTRAINT [FK_Member_Region] FOREIGN KEY ([RegionId]) REFERENCES [Region]([Id])
)