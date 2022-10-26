CREATE TABLE [dbo].[Apprentice]
(
	[MemberId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [ApprenticeId] NVARCHAR(MAX) NOT NULL, 
    [Email] NVARCHAR(MAX) NOT NULL, 
    [Name] NVARCHAR(MAX) NOT NULL, 
    [LastUpdated] DATETIME NOT NULL, 
    [IsActive] BIT NOT NULL, 
    CONSTRAINT [FK_Apprentice_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id])
)
