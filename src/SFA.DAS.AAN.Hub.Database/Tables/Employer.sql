CREATE TABLE [dbo].[Employer]
(
	[MemberId] BIGINT NOT NULL PRIMARY KEY, 
    [AccountId] BIGINT NOT NULL, 
    [UserId] BIGINT NOT NULL, 
    [Email] NVARCHAR(256) NOT NULL, 
    [Name] NVARCHAR(200) NOT NULL, 
    [LastUpdated] DATETIME NULL, 
    [IsActive] BIT NOT NULL
    CONSTRAINT [FK_Employer_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id])
)
