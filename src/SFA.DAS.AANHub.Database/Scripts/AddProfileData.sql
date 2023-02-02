 
CREATE TABLE #TempProfile
(
	[Id] INT,
    [ProfileName] NVARCHAR(200),
	[ProfileDescription] NVARCHAR(max),
	[Ordering] INT
)

INSERT INTO #TempProfile VALUES 
(1,'Networking at events in person','Events',1),
(2,'Presenting at events in person','Events',2),
(3,'Presenting at hybrid events (online and in person)','Events',3),
(4,'Presenting at online in person','Events',4),
(5,'Project management and delivery of regional events or playing a role in organising national events','Events',5),
(10,'Carrying out and writing up case studies','Promotions',1),
(11,'Designing and creating marketing materials to champion the network','Promotions',2),
(12,'Distributing communications to the network','Promotions',3),
(13,'Engaging with stakeholders to work out how to improve the network','Promotions',4),
(14,'Promoting the network on social media channels','Promotions',5),
(20,'Job title','Personal',1),
(21,'Engaged with a previous ambassador in the network','Personal',2),
(30,'Employer name','Employer',1),
(31,'Employer Address line 1','Employer',2),
(32,'Employer Address line 2','Employer',3),
(33,'Employer Town or City','Employer',4),
(34,'Employer County','Employer',5),
(35,'Employer Postcode','Employer',6)


SET IDENTITY_INSERT [dbo].[Profile] ON;

MERGE [Profile] TARGET
USING #TempProfile SOURCE ON TARGET.Id=SOURCE.Id
WHEN MATCHED THEN
	UPDATE SET 
		TARGET.ProfileName = SOURCE.ProfileName,
		TARGET.ProfileDescription = SOURCE.ProfileDescription,
		TARGET.Ordering = SOURCE.Ordering
WHEN NOT MATCHED BY TARGET THEN 
	INSERT (Id,ProfileName, ProfileDescription, Ordering)
	VALUES (SOURCE.Id,SOURCE.ProfileName, SOURCE.ProfileDescription,SOURCE.Ordering);

SET IDENTITY_INSERT [dbo].[Profile] OFF;

DROP TABLE #TempProfile