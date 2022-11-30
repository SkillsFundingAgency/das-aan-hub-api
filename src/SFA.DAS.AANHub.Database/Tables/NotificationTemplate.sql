CREATE TABLE [dbo].[NotificationTemplate]
(
	[Id] BIGINT NOT NULL IDENTITY(1, 1) PRIMARY KEY, 
    [Type] NVARCHAR(10) NOT NULL, 
    [Description] NVARCHAR(200) NOT NULL, 
    [MessageBody] NVARCHAR(256) NOT NULL, 
    [TemplateId] BIGINT NOT NULL, 
    [MergeFields] NVARCHAR(MAX) NOT NULL, 
    [IsActive] BIT NOT NULL, 
    [IsSystem] BIT NOT NULL
)
