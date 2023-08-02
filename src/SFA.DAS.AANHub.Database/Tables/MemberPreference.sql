CREATE TABLE [dbo].[MemberPreference]
(
    [Id] BIGINT IDENTITY(1,1) NOT NULL,
    [MemberId] UNIQUEIDENTIFIER NOT NULL, 
    [PreferenceId] INT NOT NULL, 
    [AllowSharing] BIT DEFAULT 0
    CONSTRAINT [PK_MemberPreference] PRIMARY KEY ([Id])
    CONSTRAINT [FK_MemberPreference_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id])
    CONSTRAINT [FK_MemberPreference_Preference] FOREIGN KEY ([PreferenceId]) REFERENCES [Preference]([Id])
);
GO

CREATE UNIQUE INDEX IXU_MemberPreference ON [MemberPreference] ([MemberId], [PreferenceId])
INCLUDE ([AllowSharing]);
GO


