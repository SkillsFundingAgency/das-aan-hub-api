CREATE VIEW [dbo].[vAuditMember]
AS
SELECT [EntityId] [MemberId], [Id] [AuditId] , [AuditTime] , [Action], [ActionedBy]
FROM [dbo].[Audit]
WHERE [Resource] = 'Member';
GO
