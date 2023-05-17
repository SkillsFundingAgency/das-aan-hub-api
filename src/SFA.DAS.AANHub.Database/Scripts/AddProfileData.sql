 
CREATE TABLE #TempProfile
(
	[Id] INT,
	[Description] NVARCHAR(200),
	[Category] NVARCHAR(20),
	[Ordering] INT,
	[UserType] NVARCHAR(200)
)

INSERT INTO #TempProfile VALUES 
(1,'Networking at events in person','Events',1, 'Apprentice'),
(2,'Presenting at events in person','Events',2, 'Apprentice'),
(3,'Presenting at hybrid events (online and in person)','Events',3, 'Apprentice'),
(4,'Presenting at online events','Events',4, 'Apprentice'),
(5,'Project management and delivery of regional events or playing a role in organising national events','Events',5, 'Apprentice'),
(10,'Carrying out and writing up case studies','Promotions',1, 'Apprentice'),
(11,'Designing and creating marketing materials to champion the network','Promotions',2, 'Apprentice'),
(12,'Distributing communications to the network','Promotions',3, 'Apprentice'),
(13,'Engaging with stakeholders to work out how to improve the network','Promotions',4, 'Apprentice'),
(14,'Promoting the network on social media channels','Promotions',5, 'Apprentice'),
(20,'Job title','Personal',1, 'Apprentice'),
(21,'Engaged with a previous ambassador in the network','Personal',2, 'Apprentice'),
(22,'Reason to join ambassador network','Personal',3, 'Apprentice'),
(30,'Employer name','Employer',1, 'Apprentice'),
(31,'Employer Address line 1','Employer',2, 'Apprentice'),
(32,'Employer Address line 2','Employer',3, 'Apprentice'),
(33,'Employer Town or City','Employer',4, 'Apprentice'),
(34,'Employer County','Employer',5, 'Apprentice'),
(35,'Employer Postcode','Employer',6, 'Apprentice'),
(36,'Employer address longitude','Employer',7, 'Apprentice'),
(37,'Employer address latitude','Employer',8, 'Apprentice'),
(41,'Meet other employer ambassadors and grow your network personally and professionally','ReasonToJoin',1, 'Employer'),
(42,'Share your knowledge, experience and best practice','ReasonToJoin',2, 'Employer'),
(43,'Project manage and deliver network events','ReasonToJoin',3, 'Employer'),
(44,'Be a role model and act as an informal mentor to other ambassadors','ReasonToJoin',4, 'Employer'),
(45,'Champion apprenticeship delivery within your networks','ReasonToJoin',5, 'Employer'),
(51,'Building apprenticeship profile of my organisation','Support',1, 'Employer'),
(52,'Increasing engagement with schools and colleges','Support',2, 'Employer'),
(53,'Getting started with apprenticeships','Support',3, 'Employer'),
(54,'Understanding training providers and resources others are using','Support',4, 'Employer'),
(55,'Using the network to best benefit my organisation','Support',5, 'Employer'),
(61,'Engaged with a previous ambassador in the network','Personal',1, 'Employer')

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

DROP TABLE #TempProfile