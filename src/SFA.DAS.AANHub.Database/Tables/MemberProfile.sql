CREATE TABLE [dbo].[MemberProfile]
(
    [MemberId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [ProfileId] INT NOT NULL, 
    [ProfileValue] NVARCHAR(200) NOT NULL
    CONSTRAINT [FK_MemberProfile_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id])
    CONSTRAINT [FK_MemberProfile_Profile] FOREIGN KEY ([ProfileId]) REFERENCES [Profile]([Id])
)
