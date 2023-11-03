CREATE TABLE [dbo].[CalendarEvent]
(
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(), 
    [CalendarId] INT NOT NULL, 
    [EventFormat] NVARCHAR(10) NOT NULL,
    [StartDate] DATETIME NOT NULL, 
    [EndDate] DATETIME NOT NULL, 
    [Title] NVARCHAR(200) NOT NULL,
    [Description] NVARCHAR(MAX) NULL, 
    [Summary] NVARCHAR(2000) NULL, 
    [RegionId] INT NULL,
    [Location] NVARCHAR(200) NULL, 
    [Postcode] NVARCHAR(10) NULL, 
    [Latitude] FLOAT NULL,
    [Longitude] FLOAT NULL,
    [URN] BIGINT NULL,
    [EventLink] NVARCHAR(2000) NULL, 
    [ContactName] NVARCHAR(200) NOT NULL, 
    [ContactEmail] NVARCHAR(256) NOT NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1,
    [PlannedAttendees] INT NULL,
    [CreatedDate] DATETIME2 DEFAULT GETUTCDATE(),
    [LastUpdatedDate] DATETIME2 NULL DEFAULT GetUTCDate(), 
    CONSTRAINT [PK_CalendarEvent] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CalendarEvent_Calendar] FOREIGN KEY ([CalendarId]) REFERENCES [Calendar]([Id])
);
GO

CREATE INDEX IX_CalendarEventSearch ON [CalendarEvent] ([IsActive], [StartDate], [EndDate], [EventFormat], [RegionId])
INCLUDE ([Title], [Summary], [Location], [Postcode], [Latitude], [Longitude], [EventLink], [ContactName], [ContactEmail]);
GO

CREATE INDEX IX_CalendarEventCreatedDate ON [CalendarEvent] ([CreatedDate], [Id])
GO

CREATE FULLTEXT CATALOG calendarevent_catalog;  

GO
CREATE FULLTEXT INDEX ON [CalendarEvent](Title)
 KEY INDEX [PK_CalendarEvent]   
		  ON calendarevent_catalog;