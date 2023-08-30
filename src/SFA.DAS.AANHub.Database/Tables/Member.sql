CREATE TABLE [dbo].[Member]
(
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [UserType] NVARCHAR(10) NOT NULL,
    [FirstName] NVARCHAR(200) NOT NULL, 
    [LastName] NVARCHAR(200) NOT NULL, 
    [Email] NVARCHAR(256) NOT NULL,
    [Status] NVARCHAR(10) NOT NULL,
    [JoinedDate] DATETIME2 NOT NULL,
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
INCLUDE ([Id], [Email], [JoinedDate], [OrganisationName], [LastUpdatedDate])
GO

CREATE UNIQUE INDEX [IX_Member_Email] ON [Member] ([Email]) INCLUDE ([Id], [FirstName], [LastName], [UserType], [Status])
GO

CREATE FULLTEXT CATALOG member_catalog;  

GO
CREATE FULLTEXT INDEX ON [Member](FullName)
 KEY INDEX [PK_Member]   
		  ON member_catalog;