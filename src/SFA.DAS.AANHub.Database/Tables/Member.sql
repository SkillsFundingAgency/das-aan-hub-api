CREATE TABLE [dbo].[Member]
(
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [UserType] NVARCHAR(10) NOT NULL,
    [FirstName] NVARCHAR(200) NOT NULL, 
    [LastName] NVARCHAR(200) NOT NULL, 
    [Email] NVARCHAR(256) NOT NULL,
    [Status] NVARCHAR(10) NOT NULL,
    [Joined] DATETIME2 NOT NULL,
    [Information] NVARCHAR(MAX) NULL,
    [RegionId] INT NULL,
    [OrganisationName] NVARCHAR(250) NULL,
    [LastUpdated] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    CONSTRAINT [PK_Member] PRIMARY KEY ([Id])
    CONSTRAINT [FK_Member_Region] FOREIGN KEY ([RegionId]) REFERENCES [Region] ([Id])
)
