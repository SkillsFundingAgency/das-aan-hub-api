CREATE TABLE [dbo].[Notification]
(
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [MemberId] UNIQUEIDENTIFIER NOT NULL, 
    [TemplateName] NVARCHAR(200) NOT NULL, 
    [Tokens] NVARCHAR(MAX) NOT NULL, 
    [CreatedBy] UNIQUEIDENTIFIER NULL,
    [CreatedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [SendAfterTime] DATETIME NULL, 
    [SentTime] DATETIME NULL, 
    CONSTRAINT [PK_Notification] PRIMARY KEY ([Id]),    
    CONSTRAINT [FK_Notifications_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id])
)
