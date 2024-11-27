
CREATE TABLE #TempRegion
(
    [Id] INT,
    [Area] NVARCHAR(100),
    [Ordering] NVARCHAR(100)
)

INSERT INTO #TempRegion VALUES 
(1, 'East Midlands', 4),
(2, 'East of England', 6),
(3, 'London', 7), 
(4, 'North East', 1),
(5, 'North West', 2),
(6, 'South East', 8),
(7, 'South West', 9),
(8, 'West Midlands', 5),
(9, 'Yorkshire and the Humber', 3)


SET IDENTITY_INSERT [dbo].[Region] ON;

MERGE Region TARGET
USING #TempRegion SOURCE ON TARGET.Id=SOURCE.Id
WHEN MATCHED THEN
  UPDATE SET 
    TARGET.Area = SOURCE.Area,
    TARGET.Ordering = SOURCE.Ordering
WHEN NOT MATCHED BY TARGET THEN 
  INSERT (Id, Area, Ordering)
  VALUES (SOURCE.Id, SOURCE.Area, SOURCE.Ordering);

SET IDENTITY_INSERT [dbo].[Region] OFF;

DROP TABLE #TempRegion
