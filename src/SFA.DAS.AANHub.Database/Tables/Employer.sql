CREATE TABLE [dbo].[Employer]
(
    [MemberId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [AccountId] BIGINT NOT NULL, 
    [UserRef] UNIQUEIDENTIFIER NOT NULL, 
    [Organisation] NVARCHAR(250) NOT NULL, 
    [Email] NVARCHAR(256) NOT NULL, 
    [Name] NVARCHAR(200) NOT NULL, 
    [LastUpdated] DATETIME NOT NULL
    CONSTRAINT [FK_Employer_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id])
    CONSTRAINT [UK_Employer] UNIQUE ([UserRef])
)
