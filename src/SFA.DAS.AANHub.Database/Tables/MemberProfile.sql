CREATE TABLE [dbo].[MemberProfile]
(
    [Id] BIGINT IDENTITY(1,1) NOT NULL,
    [MemberId] UNIQUEIDENTIFIER NOT NULL, 
    [ProfileId] INT NOT NULL, 
    [ProfileValue] NVARCHAR(200) NOT NULL
    CONSTRAINT [PK_MemberProfile] PRIMARY KEY ([Id])
    CONSTRAINT [FK_MemberProfile_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id])
    CONSTRAINT [FK_MemberProfile_Profile] FOREIGN KEY ([ProfileId]) REFERENCES [Profile]([Id])
)
