CREATE TABLE [dbo].[Notification]
(
    [Id] BIGINT NOT NULL IDENTITY(1, 1), 
    [MemberId] UNIQUEIDENTIFIER NOT NULL, 
    [TemplateName] NVARCHAR(200) NOT NULL, 
    [Tokens] NVARCHAR(MAX) NOT NULL, 
    [CreatedBy] UNIQUEIDENTIFIER NULL,
    [CreatedDate] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [SendAfterTime] DATETIME NULL, 
    [SentTime] DATETIME NULL, 
    CONSTRAINT [PK_Notification] PRIMARY KEY ([Id]),    
    CONSTRAINT [FK_Notifications_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id])
)
