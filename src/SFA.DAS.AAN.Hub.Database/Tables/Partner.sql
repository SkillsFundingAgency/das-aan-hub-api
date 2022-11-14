CREATE TABLE [dbo].[Partner]
(
	[MemberId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [UKPRN] BIGINT NOT NULL, 
    [Email] NVARCHAR(256) NULL, 
    [Name] NVARCHAR(200) NULL, 
    [LastUpdated] DATETIME NULL, 
    [IsActive] BIT NOT NULL
    CONSTRAINT [FK_Partner_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id])
)
