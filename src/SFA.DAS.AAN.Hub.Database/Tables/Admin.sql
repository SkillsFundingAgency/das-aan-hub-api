CREATE TABLE [dbo].[Admin]
(
	[MemberId] BIGINT NOT NULL PRIMARY KEY, 
    [Email] NVARCHAR(256) NOT NULL, 
    [LastUpdated] DATETIME NULL, 
    [IsActive] BIT NOT NULL
    CONSTRAINT [FK_Admin_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id])
)
