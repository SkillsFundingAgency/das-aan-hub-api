
CREATE TABLE #TempCalendar
(
    [Id] INT,
    [CalendarName] NVARCHAR(100),
    [Ordering] NVARCHAR(100)
);

INSERT INTO #TempCalendar VALUES 
(1, 'Employer engagement event', 1),
(2, 'National/Regional meeting', 2),
(3, 'School and College event', 3), 
(4, 'Careers event', 4),
(5, 'Social/Networking activity', 5),
(6, 'Training event', 6),
(7, 'Other', 7);


MERGE Calendar TARGET
USING #TempCalendar SOURCE ON TARGET.Id=SOURCE.Id
WHEN MATCHED THEN
    UPDATE SET 
        TARGET.CalendarName = SOURCE.CalendarName,
        TARGET.Ordering = SOURCE.Ordering
WHEN NOT MATCHED BY TARGET THEN 
    INSERT (Id, CalendarName, Ordering, EffectiveFromDate)
    VALUES (SOURCE.Id, SOURCE.CalendarName, SOURCE.Ordering, '2023-05-01')
WHEN NOT MATCHED BY SOURCE THEN
    DELETE;

DROP TABLE #TempCalendar;