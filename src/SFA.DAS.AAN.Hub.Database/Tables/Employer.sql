CREATE TABLE [dbo].[Employer]
(
	[MemberId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [AccountId] BIGINT NOT NULL, 
    [UserId] BIGINT NULL, 
    [Email] NVARCHAR(256) NULL, 
    [Name] NVARCHAR(200) NULL, 
    [LastUpdated] DATETIME NULL, 
    [IsActive] BIT NOT NULL
    CONSTRAINT [FK_Employer_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id])
)
