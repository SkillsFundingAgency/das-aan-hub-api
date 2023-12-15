CREATE VIEW [dbo].[vAuditMemberProfile] 
AS
SELECT [EntityId] [MemberId], [Id] [AuditId] , [AuditTime] , [Action], [ActionedBy]
FROM [dbo].[Audit]
WHERE [Resource] = 'MemberProfile';
GO
