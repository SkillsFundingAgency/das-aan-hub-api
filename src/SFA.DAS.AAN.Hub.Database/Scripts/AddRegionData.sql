
CREATE TABLE #TempRegion
(
	[Id] INT,
    [Area] NVARCHAR(100),
	[Ordering] NVARCHAR(100)
)

INSERT INTO #TempRegion VALUES 
(1, 'East Midlands', 1),
(2, 'East of England', 2),
(3, 'London', 3), 
(4, 'North East', 4),
(5, 'North West', 5),
(6, 'South East', 6),
(7, 'South West', 7),
(8, 'West Midlands', 8),
(9, 'Yorkshire and the Humber', 9)


SET IDENTITY_INSERT [dbo].[Region] ON;

MERGE Region TARGET
USING #TempRegion SOURCE
ON TARGET.Id=SOURCE.Id
WHEN MATCHED THEN
UPDATE SET TARGET.Area = SOURCE.Area,
Ordering = SOURCE.Ordering
WHEN NOT MATCHED BY TARGET THEN 
INSERT (Id,Area, Ordering)
VALUES (SOURCE.Id,SOURCE.Area, SOURCE.Ordering);

SET IDENTITY_INSERT [dbo].[Region] OFF;
DROP TABLE #TempRegion
