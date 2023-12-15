CREATE VIEW [dbo].[vAuditCalendarEvent]
AS
SELECT [EntityId] [CalendarEventid], [Id] [AuditId] , [AuditTime] , [Action], [ActionedBy]
FROM [dbo].[Audit]
WHERE [Resource] = 'CalendarEvent';
GO