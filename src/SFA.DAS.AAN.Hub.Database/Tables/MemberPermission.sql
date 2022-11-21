CREATE TABLE [dbo].[MemberPermission]
(
	[MemberId] UNIQUEIDENTIFIER NOT NULL, 
    [PermissionId] BIGINT NOT NULL, 
    [IsActive] BIT NOT NULL
    CONSTRAINT [FK_MemberPermission_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id])
    CONSTRAINT [FK_MemberPermission_Permission] FOREIGN KEY ([PermissionId]) REFERENCES [Permission]([Id])
)
GO

ALTER TABLE dbo.MemberPermission ADD CONSTRAINT
	PK_MemberPermission PRIMARY KEY CLUSTERED 
	(MemberId, PermissionId)
