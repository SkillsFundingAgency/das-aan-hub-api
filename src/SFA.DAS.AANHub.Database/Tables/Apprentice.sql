CREATE TABLE [dbo].[Apprentice]
(
	[MemberId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [ApprenticeId] BIGINT NOT NULL, 
    [Email] NVARCHAR(256) NULL, 
    [Name] NVARCHAR(200) NULL, 
    [LastUpdated] DATETIME NULL, 
    [IsActive] BIT NOT NULL, 
    CONSTRAINT [FK_Apprentice_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id])
)
