UPDATE CE
SET CE.CreatedByMemberId = A.ActionedBy
FROM [dbo].[CalendarEvent] CE
INNER JOIN [dbo].[Audit] A
    ON CE.Id = A.EntityId
WHERE A.Action = 'Create' AND A.Resource='CalendarEvent'
AND CE.CreatedByMemberId IS NULL;  

