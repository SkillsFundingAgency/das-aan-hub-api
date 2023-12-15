CREATE VIEW [dbo].[vAuditApprentice] 
AS
SELECT [EntityId] [MemberId], [Id] [AuditId] , [AuditTime] , [Action], [ActionedBy]
FROM [dbo].[Audit]
WHERE [Resource] = 'Apprentice';
GO