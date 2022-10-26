CREATE TABLE [dbo].[Employer]
(
	[MemberId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [AccountId] NVARCHAR(MAX) NOT NULL, 
    [UserId] NVARCHAR(MAX) NOT NULL, 
    [Email] NVARCHAR(MAX) NOT NULL, 
    [Name] NVARCHAR(MAX) NOT NULL, 
    [LastUpdated] DATETIME NOT NULL, 
    [IsActive] BIT NOT NULL
    CONSTRAINT [FK_Employer_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id])
)
