﻿CREATE TABLE [dbo].[JobAudit]
(
    [Id] INT NOT NULL IDENTITY(1, 1),
	[JobName] NVARCHAR(250) NOT NULL , 
    [StartTime] DATETIME2 NOT NULL, 
    [EndTime] DATETIME2 NOT NULL,
    [Notes] NVARCHAR(MAX) NULL,
    CONSTRAINT [PK_JobAudit] PRIMARY KEY ([Id])
)
GO

CREATE NONCLUSTERED INDEX [IX_JobAudit_JobNameStartTime] ON [dbo].[JobAudit] ([JobName], [StartTime]);