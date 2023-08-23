CREATE TABLE [dbo].[Notification]
(
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [MemberId] UNIQUEIDENTIFIER NOT NULL,
    [TemplateName] NVARCHAR(200) NOT NULL,
    [Tokens] NVARCHAR(MAX) NOT NULL,
    [CreatedBy] UNIQUEIDENTIFIER NOT NULL,
    [CreatedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [SendAfterTime] DATETIME NULL,
    [SentTime] DATETIME NULL,
    [IsSystem] BIT NOT NULL,
    [ReferenceId] varchar(36) NULL,
    CONSTRAINT [PK_Notification] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Notifications_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id])
)

GO

CREATE INDEX [IX_Notifications] ON [dbo].[Notification](CreatedDate, IsSystem)
INCLUDE (Id, MemberId, TemplateName, Tokens, CreatedBy, SendAfterTime, SentTime, ReferenceId)