CREATE VIEW [dbo].[vAuditAttendance] 
AS
SELECT [EntityId] [AttendanceId], [Id] [AuditId] , [AuditTime] , [Action], [ActionedBy]
FROM [dbo].[Audit]
WHERE [Resource] = 'Attendance';
GO