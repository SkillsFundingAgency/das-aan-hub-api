﻿CREATE VIEW [dbo].[vAuditAdmin]
AS
SELECT [EntityId] [MemberId], [Id] [AuditId] , [AuditTime] , [Action], [ActionedBy]
FROM [dbo].[Audit]
WHERE [Resource] = 'Admin';
GO
