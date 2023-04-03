CREATE TABLE [dbo].[EventGuest]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
    [CalendarEventId] UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES [CalendarEvent]([Id]),  
    [GuestName] NVARCHAR(200) NULL,
    [GuestJobTitle] NVARCHAR(200) NULL
);
GO

CREATE UNIQUE INDEX IX_EventGuest_CalendarEventId ON  [dbo].[EventGuest] ([CalendarEventId])
INCLUDE ([Id], [GuestName], [GuestJobTitle]);
GO
