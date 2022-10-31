CREATE TABLE [dbo].[Notifications]
(
	[Id] BIGINT NOT NULL IDENTITY(1, 1) PRIMARY KEY, 
    [MemberId] BIGINT NOT NULL, 
    [NotificationTemplateId] BIGINT NOT NULL, 
    [MergeValues] NVARCHAR(MAX) NOT NULL, 
    [CreatedBy] BIGINT NULL, 
    [Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [SentTime] DATETIME NOT NULL, 
    [Viewed] BIT NOT NULL, 
    [ViewedTime] DATETIME NOT NULL
)
