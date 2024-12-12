CREATE TABLE [dbo].[MemberNotificationEventFormat]
(
    [Id] BIGINT IDENTITY(1,1) NOT NULL,
    [MemberId] UNIQUEIDENTIFIER NOT NULL,
    [EventFormat] NVARCHAR(10) NOT NULL,
    [Ordering] INT NOT NULL,
    [ReceiveNotifications] BIT NOT NULL DEFAULT 0,
    CONSTRAINT [PK_MemberNotificationEventFormat] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_MemberNotificationEventFormat_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id])
);
GO