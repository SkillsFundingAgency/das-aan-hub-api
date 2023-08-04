CREATE TABLE [dbo].[NotificationTemplate]
(
    [Id] BIGINT NOT NULL, 
    [Description] NVARCHAR(200) NOT NULL, 
    [TemplateName] NVARCHAR(200) NOT NULL, 
    [MergeFields] NVARCHAR(MAX) NOT NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1, 
    CONSTRAINT [PK_NotificationTemplate] PRIMARY KEY ([Id])        
)
