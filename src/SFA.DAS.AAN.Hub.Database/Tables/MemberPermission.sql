CREATE TABLE [dbo].[MemberPermission]
(
	[MemberId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [PermissionId] BIGINT NOT NULL, 
    [IsActive] BIT NOT NULL
    CONSTRAINT [FK_MemberPermission_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id])
    CONSTRAINT [FK_MemberPermission_Permission] FOREIGN KEY ([PermissionId]) REFERENCES [Permission]([Id])
)
