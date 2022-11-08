CREATE TABLE [dbo].[MemberPermission]
(
	[MemberId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [PermissionId] BIGINT NOT NULL, 
    [IsActive] BIT NOT NULL
)
