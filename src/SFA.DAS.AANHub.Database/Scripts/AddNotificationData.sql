CREATE TABLE #NotificationTemplate
(
    [Id] BIGINT NOT NULL, 
    [Description] NVARCHAR(200) NOT NULL, 
    [TemplateName] NVARCHAR(200) NOT NULL, 
    [MergeFields] NVARCHAR(MAX) NOT NULL, 
    [IsActive] BIT NOT NULL,   
);


INSERT INTO #NotificationTemplate
VALUES 
(1,'ask for industry advice','AANIndustryAdvice','"FirstName"',1),
(2,'ask for help with a network activity','AANAskForHelp','"FirstName"',1),
(3,'request a case study','AANRequestCaseStudy','"FirstName"',1),
(4,'get in touch after meeting at a network event','AANGetInTouch','"FirstName"',1);


MERGE INTO [dbo].[NotificationTemplate] src
USING #NotificationTemplate upd
ON (src.[id] = upd.[Id])
WHEN MATCHED THEN 
UPDATE SET src.[Description] = upd.[Description],
           src.[TemplateName] = upd.[TemplateName],
           src.[MergeFields] = upd.[MergeFields],
           src.[IsActive] = upd.[IsActive]
WHEN NOT MATCHED THEN 
INSERT ([Id],[Description],[TemplateName],[MergeFields],[IsActive])
VALUES (upd.[Id],upd.[Description],upd.[TemplateName],upd.[MergeFields],upd.[IsActive])
WHEN NOT MATCHED BY SOURCE THEN 
UPDATE SET src.[IsActive] = 0
;

DROP TABLE #NotificationTemplate;
