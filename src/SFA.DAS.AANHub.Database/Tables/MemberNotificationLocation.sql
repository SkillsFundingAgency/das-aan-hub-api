CREATE TABLE [dbo].[MemberNotificationLocation]
(
    [Id] BIGINT IDENTITY(1,1) NOT NULL,
    [MemberId] UNIQUEIDENTIFIER NOT NULL,
    [Name] NVARCHAR(200) NOT NULL,
    [Radius] INT NOT NULL,
    [Latitude] FLOAT NOT NULL,
    [Longitude] FLOAT NOT NULL,
    CONSTRAINT [PK_MemberNotificationLocation] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_MemberNotificationLocation_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id])
);
GO