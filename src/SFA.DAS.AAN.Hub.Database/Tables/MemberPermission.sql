CREATE TABLE [dbo].[MemberPermission]
(
	[MemberId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [PermissionId] NVARCHAR(MAX) NOT NULL, 
    [IsActive] BIT NOT NULL
)
