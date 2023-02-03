 
CREATE TABLE #TempProfile
(
	[Id] INT,
	[Description] NVARCHAR(200),
	[Category] NVARCHAR(20),
	[Ordering] INT,
	[UserType] NVARCHAR(200)
)

INSERT INTO #TempProfile VALUES 
(1,'Networking at events in person','Events',1, 'Apprentis'),
(2,'Presenting at events in person','Events',2, 'Apprentis'),
(3,'Presenting at hybrid events (online and in person)','Events',3, 'Apprentis'),
(4,'Presenting at online in person','Events',4, 'Apprentis'),
(5,'Project management and delivery of regional events or playing a role in organising national events','Events',5, 'Apprentis'),
(10,'Carrying out and writing up case studies','Promotions',1, 'Apprentis'),
(11,'Designing and creating marketing materials to champion the network','Promotions',2, 'Apprentis'),
(12,'Distributing communications to the network','Promotions',3, 'Apprentis'),
(13,'Engaging with stakeholders to work out how to improve the network','Promotions',4, 'Apprentis'),
(14,'Promoting the network on social media channels','Promotions',5, 'Apprentis'),
(20,'Job title','Personal',1, 'Apprentis'),
(21,'Engaged with a previous ambassador in the network','Personal',2, 'Apprentis'),
(30,'Employer name','Employer',1, 'Apprentis'),
(31,'Employer Address line 1','Employer',2, 'Apprentis'),
(32,'Employer Address line 2','Employer',3, 'Apprentis'),
(33,'Employer Town or City','Employer',4, 'Apprentis'),
(34,'Employer County','Employer',5, 'Apprentis'),
(35,'Employer Postcode','Employer',6, 'Apprentis')


SET IDENTITY_INSERT [dbo].[Profile] ON;

MERGE [Profile] TARGET
USING #TempProfile SOURCE ON TARGET.Id=SOURCE.Id
WHEN MATCHED THEN
	UPDATE SET
		TARGET.[Description] = SOURCE.[Description],
		TARGET.Category = SOURCE.Category,
		TARGET.Ordering = SOURCE.Ordering,
		TARGET.UserType = SOURCE.UserType
WHEN NOT MATCHED BY TARGET THEN 
	INSERT (Id, Description, Category, Ordering, UserType)
	VALUES (SOURCE.Id, SOURCE.Description, SOURCE.Category, SOURCE.Ordering, SOURCE.UserType);

SET IDENTITY_INSERT [dbo].[Profile] OFF;

DROP TABLE #TempProfile