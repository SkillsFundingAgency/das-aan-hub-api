CREATE TABLE #TempPermission
(
	[Id] INT,
    [Name] NVARCHAR(200),
	[Description] NVARCHAR(200)
)
 
INSERT INTO #TempPermission VALUES 
(1,'AAN Administrator','Allowed to administer AAN system'),
(2,'ASK Regional','Allowed to manage Events')


SET IDENTITY_INSERT [dbo].[Permission] ON;

MERGE Permission TARGET
USING #TempPermission SOURCE ON TARGET.Id=SOURCE.Id
WHEN MATCHED THEN
	UPDATE SET 
		TARGET.[Name] = SOURCE.[Name],
		TARGET.[Description] = SOURCE.[Description]
WHEN NOT MATCHED BY TARGET THEN 
	INSERT (Id, Name, Description)
	VALUES (SOURCE.Id,SOURCE.Name, SOURCE.Description);

SET IDENTITY_INSERT [dbo].[Permission] OFF;

DROP TABLE #TempPermission