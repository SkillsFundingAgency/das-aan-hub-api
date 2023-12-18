-- use this to fix missing Audit data
MERGE INTO [dbo].[Audit] src
USING (
-- the CalendarEvent Audit Record
SELECT ce1.[Id] EntityId, ab1.[Id] AuditId
FROM [dbo].[CalendarEvent] ce1
JOIN (
SELECT JSON_VALUE([After],'$.StartDate') StartDate
,JSON_VALUE([After],'$.EndDate') EndDate
,JSON_VALUE([After],'$.Id') CalendarEventId
,CONVERT(char(19),AuditTime) AuditDateTime
,* 
FROM [dbo].[Audit]
WHERE [Resource] = 'CalendarEvent'  AND JSON_VALUE([After],'$.Id') IS NULL
) ab1 ON ce1.[StartDate] = ab1.[StartDate] 
AND ce1.[EndDate] = ab1.[EndDate] 
AND (CalendarEventId IS NULL OR ce1.[Id] != CalendarEventId)
AND CONVERT(char(19),CreatedDate) = ab1.AuditDateTime
UNION
SELECT EntityId, AuditId 
FROM 
(
SELECT [Id] AuditId,
CASE WHEN [Resource] IN ('Apprentice','Employer') 
     THEN JSON_VALUE([After],'$.MemberId') 
     ELSE JSON_VALUE([After],'$.Id') END EntityId
FROM [dbo].[Audit]
WHERE 1=1 --[EntityId] IS NULL
AND [Resource] != 'EventGuest'
) ab2 WHERE EntityId IS NOT NULL
) upd 
ON (src.[Id] = upd.[AuditId])
WHEN MATCHED THEN UPDATE SET src.[EntityId] = upd.[EntityId];