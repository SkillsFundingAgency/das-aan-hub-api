
CREATE TABLE #TempCalendar
(
    [Id] INT,
    [CalendarName] NVARCHAR(100),
    [Ordering] NVARCHAR(100)
);

INSERT INTO #TempCalendar VALUES 
(1, 'School event', 1),
(2, 'College event', 2),
(3, 'Training', 3), 
(4, 'Regional meetings', 4),
(5, 'National meetings', 5),
(6, 'Regionally hosted/sponsored events', 6),
(7, 'Social and networking events', 7),
(8, 'Other', 8);


MERGE Calendar TARGET
USING #TempCalendar SOURCE ON TARGET.Id=SOURCE.Id
WHEN MATCHED THEN
    UPDATE SET 
        TARGET.CalendarName = SOURCE.CalendarName,
        TARGET.Ordering = SOURCE.Ordering
WHEN NOT MATCHED BY TARGET THEN 
    INSERT (Id, CalendarName, Ordering, EffectiveFrom)
    VALUES (SOURCE.Id, SOURCE.CalendarName, SOURCE.Ordering, '2023-05-01')
WHEN NOT MATCHED BY SOURCE THEN
    DELETE;

DROP TABLE #TempCalendar;