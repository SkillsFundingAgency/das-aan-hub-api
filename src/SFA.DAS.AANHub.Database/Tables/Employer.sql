CREATE TABLE [dbo].[Employer]
(
    [MemberId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [AccountId] BIGINT NOT NULL, 
    [UserRef] UNIQUEIDENTIFIER NOT NULL
    CONSTRAINT [FK_Employer_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id])
    CONSTRAINT [UK_Employer] UNIQUE ([UserRef])
)
