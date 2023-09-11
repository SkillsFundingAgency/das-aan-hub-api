
CREATE TABLE #TempLeavingReason
(
    [Id] INT NOT NULL,
    [Category] NVARCHAR(100) NULL, 
    [Description] NVARCHAR(200) NULL, 
    [Ordering] INT NULL
);


INSERT INTO #TempLeavingReason VALUES 
(1, 'What are your reasons for leaving the network', 'I am unable to commit the time required to deliver my role', 1),
(2, 'What are your reasons for leaving the network','I have been promoted', 2),
(3, 'What are your reasons for leaving the network','I have moved organisation', 3),
(4, 'What are your reasons for leaving the network','I have moved organisation and my employer will not support my role as a network ambassador', 4),
(5, 'What are your reasons for leaving the network','I didn''t get enough opportunities to take part in events and network activity', 5),
(6, 'What are your reasons for leaving the network','I didn''t get enough in my role as an ambassador', 6),
(7, 'What are your reasons for leaving the network','The ambassador role isn''t what I expected', 7),
(8, 'What are your reasons for leaving the network','The Apprenticeship Ambassador Network isn''t what I expected', 8),
(9, 'What are your reasons for leaving the network','I have moved location and do not wish to join a new region', 9),
(10, 'What are your reasons for leaving the network','Other', 10),
(11, 'Which of the following did you benefit from while you were a member', 'Professional networking', 1),
(12, 'Which of the following did you benefit from while you were a member', 'Developing confidence, communication or presentation skills', 2),
(13, 'Which of the following did you benefit from while you were a member', 'Having the opportunity to tell my story', 3),
(14, 'Which of the following did you benefit from while you were a member', 'Attending useful events (online or face to face)', 4),
(15, 'Which of the following did you benefit from while you were a member', 'Meeting other ambassadors and developing knowledge of other sectors', 5),
(21, 'What was your experience of the AAN portal in enhancing your role as an ambassador', 'Excellent', 1),
(22, 'What was your experience of the AAN portal in enhancing your role as an ambassador', 'Good', 2),
(23, 'What was your experience of the AAN portal in enhancing your role as an ambassador', 'Average', 3),
(24, 'What was your experience of the AAN portal in enhancing your role as an ambassador', 'Poor', 4);


MERGE [dbo].[LeavingReason] TARGET
USING #TempLeavingReason SOURCE 
ON TARGET.Id=SOURCE.Id
WHEN MATCHED THEN
    UPDATE SET TARGET.[Category] = SOURCE.[Category],
               TARGET.[Description] = SOURCE.[Description],
               TARGET.[Ordering] = SOURCE.[Ordering]               
WHEN NOT MATCHED BY TARGET THEN 
    INSERT (Id, [Category], [Description], [Ordering])
    VALUES (SOURCE.Id, SOURCE.[Category], SOURCE.[Description], SOURCE.[Ordering]);

DROP TABLE #TempLeavingReason
