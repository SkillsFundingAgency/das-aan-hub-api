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
    [LastUpdatedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    CONSTRAINT [PK_Member] PRIMARY KEY ([Id])
    CONSTRAINT [FK_Member_Region] FOREIGN KEY ([RegionId]) REFERENCES [Region] ([Id])
)
