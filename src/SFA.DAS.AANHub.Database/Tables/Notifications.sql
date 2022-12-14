CREATE TABLE [dbo].[Notifications]
(
	[Id] BIGINT NOT NULL IDENTITY(1, 1) PRIMARY KEY, 
    [MemberId] UNIQUEIDENTIFIER NOT NULL, 
    [NotificationTemplateId] BIGINT NOT NULL, 
    [MergeValues] NVARCHAR(MAX) NOT NULL, 
    [CreatedBy] BIGINT NULL, 
    [Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [SentTime] DATETIME NOT NULL, 
    [Viewed] BIT NOT NULL, 
    [ViewedTime] DATETIME NOT NULL
    CONSTRAINT [FK_Notifications_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id])
    CONSTRAINT [FK_Notifications_NotificationTemplate] FOREIGN KEY ([NotificationTemplateId]) REFERENCES [NotificationTemplate]([Id])
)
