CREATE TABLE [dbo].[LeavingReason]
(
    [Id] INT NOT NULL,
    [Category] NVARCHAR(100) NOT NULL, 
    [Description] NVARCHAR(200) NOT NULL, 
    [Ordering] INT NOT NULL,
    CONSTRAINT [PK_LeavingReason] PRIMARY KEY ([Id])
);
GO
