CREATE TABLE [dbo].[MemberPermission]
(
	[MemberId] BIGINT NOT NULL PRIMARY KEY, 
    [PermissionId] BIGINT NOT NULL, 
    [IsActive] BIT NOT NULL
)
