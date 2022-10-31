CREATE TABLE [dbo].[Partner]
(
	[MemberId] BIGINT NOT NULL PRIMARY KEY, 
    [UKPRN] BIGINT NOT NULL, 
    [Email] NVARCHAR(256) NOT NULL, 
    [Name] NVARCHAR(200) NOT NULL, 
    [LastUpdated] DATETIME NULL, 
    [IsActive] BIT NOT NULL
    CONSTRAINT [FK_Partner_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id])
)
