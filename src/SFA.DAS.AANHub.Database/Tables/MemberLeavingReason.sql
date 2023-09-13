CREATE TABLE [dbo].[MemberLeavingReason]
(
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [MemberId] UNIQUEIDENTIFIER NOT NULL,
    [LeavingReasonId] INT NOT NULL,
    CONSTRAINT [PK_MemberLeavingReason] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_MemberLeavingReason_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id]),
    CONSTRAINT [FK_MemberLeavingReason_LeavingReasonId] FOREIGN KEY ([LeavingReasonId]) REFERENCES [LeavingReason]([Id])
)
GO
