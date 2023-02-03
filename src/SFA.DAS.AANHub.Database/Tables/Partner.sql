CREATE TABLE [dbo].[Partner]
(
	[MemberId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Organisation] NVARCHAR(200) NOT NULL,
    [Email] NVARCHAR(256) NOT NULL, 
    [UserName] NVARCHAR(200) NOT NULL, 
    [LastUpdated] DATETIME NOT NULL
    CONSTRAINT [FK_Partner_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id])
)
