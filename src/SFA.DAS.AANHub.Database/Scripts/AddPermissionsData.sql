CREATE TABLE #TempPermission
(
	[Id] INT,
    [PermissionName] NVARCHAR(200),
	[PermissionDescription] NVARCHAR(200)
)
 
INSERT INTO #TempPermission VALUES 
(1,'AAN Administrator','Allowed to administer AAN system'),
(2,'ASK Regional','Allowed to manage Events')


SET IDENTITY_INSERT [dbo].[Permission] ON;

MERGE Permission TARGET
USING #TempPermission SOURCE ON TARGET.Id=SOURCE.Id
WHEN MATCHED THEN
	UPDATE SET 
		TARGET.PermissionName = SOURCE.PermissionName,
		TARGET.PermissionDescription = SOURCE.PermissionDescription
WHEN NOT MATCHED BY TARGET THEN 
	INSERT (Id,PermissionName, PermissionDescription)
	VALUES (SOURCE.Id,SOURCE.PermissionName, SOURCE.PermissionDescription);

SET IDENTITY_INSERT [dbo].[Permission] OFF;

DROP TABLE #TempPermission