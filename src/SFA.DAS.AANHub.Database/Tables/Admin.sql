CREATE TABLE [dbo].[Admin]
(
	[MemberId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Email] NVARCHAR(256) NULL, 
    [LastUpdated] DATETIME NULL, 
    [IsActive] BIT NOT NULL
    CONSTRAINT [FK_Admin_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id])
)
