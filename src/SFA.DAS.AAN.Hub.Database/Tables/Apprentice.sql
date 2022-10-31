CREATE TABLE [dbo].[Apprentice]
(
	[MemberId] BIGINT NOT NULL PRIMARY KEY, 
    [ApprenticeId] BIGINT NOT NULL, 
    [Email] NVARCHAR(256) NOT NULL, 
    [Name] NVARCHAR(200) NOT NULL, 
    [LastUpdated] DATETIME NULL, 
    [IsActive] BIT NOT NULL, 
    CONSTRAINT [FK_Apprentice_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id])
)
