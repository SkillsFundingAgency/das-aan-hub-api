CREATE TABLE [dbo].[CalendarEvent]
(
	  [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
    [CalendarId] BIGINT NOT NULL, 
    [CreatedByUserId] UNIQUEIDENTIFIER NOT NULL, 
    [Start] DATETIME NOT NULL, 
    [End] DATETIME NOT NULL, 
    [Description] NVARCHAR(200) NOT NULL,
    [RegionId] INT NULL,
    [Location] NVARCHAR(200) NULL, 
    [Postcode] NVARCHAR(10) NULL, 
    [EventLink] NCHAR(200) NULL, 
    [Contact] NCHAR(200) NOT NULL, 
    [ContactEmail] NCHAR(256) NOT NULL, 
    [UpdatedByUserId] UNIQUEIDENTIFIER NOT NULL, 
    [Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [Updated] DATETIME NULL, 
    [Deleted] DATETIME NULL, 
    [IsActive] BIT NOT NULL
    CONSTRAINT [FK_CalendarEvent_Calendar] FOREIGN KEY ([CalendarId]) REFERENCES [Calendar]([Id])
    CONSTRAINT [FK_CalendarEvent_Region] FOREIGN KEY ([RegionId]) REFERENCES [Region]([Id])
)
