CREATE TABLE [dbo].[CalendarEvent]
(
	[Id] BIGINT NOT NULL IDENTITY(1, 1) PRIMARY KEY, 
    [CalendarId] BIGINT NOT NULL, 
    [Start] DATETIME NOT NULL, 
    [End] DATETIME NOT NULL, 
    [Description] NVARCHAR(200) NOT NULL, 
    [Location] NVARCHAR(200) NULL, 
    [Postcode] NVARCHAR(10) NULL, 
    [EventLink] NCHAR(200) NULL, 
    [Contact] NCHAR(200) NOT NULL, 
    [ContactEmail] NCHAR(256) NOT NULL, 
    [IsActive] BIT NOT NULL
    CONSTRAINT [FK_CalendarEvent_Calendar] FOREIGN KEY ([CalendarId]) REFERENCES [Calendar]([Id])
)
