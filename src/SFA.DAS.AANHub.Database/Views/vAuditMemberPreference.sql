CREATE VIEW [dbo].[vAuditMemberPreference] 
AS
SELECT [EntityId] [MemberId], [Id] [AuditId] , [AuditTime] , [Action], [ActionedBy]
FROM [dbo].[Audit]
WHERE [Resource] = 'MemberPreference';
GO
