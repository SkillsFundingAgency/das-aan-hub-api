CREATE TABLE #TempMember
(
	[Id] uniqueidentifier,
    [UserType] NVARCHAR(10),
	[Status] NVARCHAR(10),
	[Joined] DateTime
)

INSERT INTO #TempMember VALUES 
     ('B46C2A2A-E35C-4788-B4B7-1F7E84081846', 'Apprentice', 'Live', GetDate())

MERGE Member TARGET
USING #TempMember SOURCE
ON TARGET.Id=SOURCE.Id
WHEN MATCHED THEN
UPDATE SET TARGET.UserType = SOURCE.UserType,
[Status] = SOURCE.[Status],
Joined = SOURCE.Joined
WHEN NOT MATCHED BY TARGET THEN 
INSERT (Id,UserType,[Status],Joined)
VALUES (SOURCE.Id, SOURCE.UserType, SOURCE.[Status], SOURCE.Joined);

DROP TABLE #TempMember