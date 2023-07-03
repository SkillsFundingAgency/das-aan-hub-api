
CREATE TABLE #TempPreference
(
    [Id] INT NOT NULL,
    [Group] NVARCHAR(200) NOT NULL 
);

INSERT INTO #TempPreference VALUES
(1,'Areas of Interest'),
(2,'Job Title'),
(3,'Employer Name'),
(4,'Employer Address'),
(5,'Region where you work'),
(6,'Apprenticeship'),
(7,'Sector'),
(8,'Region where you live'),
(9,'Biography'),
(10,'LinkedIn');

MERGE [Preference] TARGET
USING #TempPreference SOURCE ON TARGET.Id=SOURCE.Id
WHEN MATCHED THEN
    UPDATE SET TARGET.[Group] = SOURCE.[Group]
WHEN NOT MATCHED BY TARGET THEN 
    INSERT (Id, [Group])
    VALUES (SOURCE.Id, SOURCE.[Group]);

DROP TABLE #TempPreference
