CREATE TABLE [dbo].[Audit]
(
    [Id] BIGINT NOT NULL IDENTITY(1, 1) PRIMARY KEY, 
    [AuditTime] DATETIME2 NOT NULL, 
    [ActionedBy] UNIQUEIDENTIFIER NOT NULL, 
    [Action] NVARCHAR(50) NOT NULL, 
    [Resource] NVARCHAR(256) NOT NULL, 
    [Before] NVARCHAR(MAX) NULL, 
    [After] NVARCHAR(MAX) NULL,
    [EntityId] UniqueIdentifier NULL,
    CONSTRAINT [FK_Audit_Member] FOREIGN KEY ([ActionedBy]) REFERENCES [Member]([Id])
)

GO 

CREATE INDEX IX_AuditResourceActionedByEntityId ON [dbo].[Audit] ( [Resource], [ActionedBy], [EntityId]) INCLUDE ( [Id] , [AuditTime] , [Action])
GO

CREATE INDEX IX_AuditActionedBy ON [dbo].[Audit] ([ActionedBy]) INCLUDE ([Id], [Resource])
