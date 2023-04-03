CREATE TABLE [dbo].[CalendarEvent]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
    [CalendarId] INT NOT NULL, 
    [EventFormat] NVARCHAR(10) NOT NULL,
    [Start] DATETIME NOT NULL, 
    [End] DATETIME NOT NULL, 
    [Description] NVARCHAR(200) NOT NULL, 
    [Summary] NVARCHAR(max) NULL, 
    [RegionId] INT NULL,
    [Location] NVARCHAR(200) NULL, 
    [Postcode] NVARCHAR(10) NULL, 
    [Latitude] FLOAT NULL,
    [Longitude] FLOAT NULL,    
    [EventLink] NVARCHAR(2000) NULL, 
    [ContactName] NVARCHAR(200) NOT NULL, 
    [ContactEmail] NVARCHAR(256) NOT NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CancelReason] NVARCHAR(max) NULL
);
GO

CREATE INDEX IX_CalendarEventSearch ON [CalendarEvent] ([IsActive], [Start], [End], [EventFormat], [RegionId])
INCLUDE ([Description], [Location], [Postcode], [Latitude], [Longitude], [EventLink], [ContactName], [ContactEmail]);
GO
