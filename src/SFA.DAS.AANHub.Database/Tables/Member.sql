CREATE TABLE [dbo].[Member]
(
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [UserType] NVARCHAR(10) NOT NULL,
    [FirstName] NVARCHAR(200) NOT NULL, 
    [LastName] NVARCHAR(200) NOT NULL, 
    [Email] NVARCHAR(256) NOT NULL,
    [Status] NVARCHAR(10) NOT NULL,
    [JoinedDate] DATETIME2 NOT NULL,
    [EndDate] DATETIME2 NULL,
    [RegionId] INT NULL,
    [OrganisationName] NVARCHAR(250) NULL,
    [LastUpdatedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [IsRegionalChair] BIT DEFAULT 0,
    [FullName] AS CONVERT(NVARCHAR(400), TRIM([FirstName]) + ' ' + TRIM([LastName])),
    CONSTRAINT [PK_Member] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Member_Region] FOREIGN KEY ([RegionId]) REFERENCES [Region] ([Id])
)
GO

CREATE INDEX [IX_Member_Search] ON [Member] ([UserType], [Status], [FullName], [RegionId]) 
INCLUDE ([Id], [Email], [JoinedDate], [EndDate], [OrganisationName], [LastUpdatedDate])
GO

CREATE UNIQUE INDEX [IX_Member_Email] ON [Member] ([Email]) INCLUDE ([Id], [FullName], [UserType], [Status])
GO
