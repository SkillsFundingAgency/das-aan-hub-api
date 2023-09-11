
CREATE TABLE #TempPreference
(
    [Id] INT NOT NULL,
    [Group] NVARCHAR(200) NOT NULL 
);

INSERT INTO #TempPreference VALUES
(1,'Job Title'),
(2,'Region where you work'),
(3,'Biography'),
(4,'LinkedIn');

MERGE [Preference] TARGET
USING #TempPreference SOURCE ON TARGET.Id=SOURCE.Id
WHEN MATCHED THEN
    UPDATE SET TARGET.[Group] = SOURCE.[Group]
WHEN NOT MATCHED BY TARGET THEN 
    INSERT (Id, [Group])
    VALUES (SOURCE.Id, SOURCE.[Group]);

DROP TABLE #TempPreference
