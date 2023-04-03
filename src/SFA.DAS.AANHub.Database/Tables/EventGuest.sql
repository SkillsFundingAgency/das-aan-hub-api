CREATE TABLE [dbo].[EventGuest]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
    [CalendarEventId] UNIQUEIDENTIFIER NOT NULL,  
    [GuestName] NVARCHAR(200) NULL,
    [GuestJobTitle] NVARCHAR(200) NULL,
    CONSTRAINT [FK_EventGuest_CalendarEventId] FOREIGN KEY ([CalendarEventId]) REFERENCES [CalendarEvent]([Id]),  
);
GO

CREATE UNIQUE INDEX IX_EventGuest_CalendarEventId ON  [EventGuest] ([CalendarEventId])
INCLUDE ([Id], [GuestName], [GuestJobTitle]);
GO
