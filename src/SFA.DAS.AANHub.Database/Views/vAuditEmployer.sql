CREATE VIEW [dbo].[vAuditEmployer]
AS
SELECT [EntityId] [MemberId], [Id] [AuditId] , [AuditTime] , [Action], [ActionedBy]
FROM [dbo].[Audit]
WHERE [Resource] = 'Employer';
GO
