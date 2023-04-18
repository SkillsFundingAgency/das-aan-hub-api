CREATE TABLE [dbo].[Member]
(
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [UserType] NVARCHAR(10) NOT NULL,
    [Status] NVARCHAR(10) NOT NULL,
    [Information] NVARCHAR(MAX) NULL,
    [Joined] DATETIME NOT NULL,
    [RegionId] INT NULL
    CONSTRAINT [PK_Member] PRIMARY KEY ([Id])
    CONSTRAINT [FK_Member_Region] FOREIGN KEY ([RegionId]) REFERENCES [Region] ([Id])
)
