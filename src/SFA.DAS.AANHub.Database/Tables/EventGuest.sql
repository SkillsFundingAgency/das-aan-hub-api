CREATE TABLE [dbo].[EventGuest]
(
    [Id] BIGINT NOT NULL IDENTITY(1,1), 
    [CalendarEventId] UNIQUEIDENTIFIER NOT NULL,  
    [GuestName] NVARCHAR(200) NULL,
    [GuestJobTitle] NVARCHAR(200) NULL,
    CONSTRAINT [PK_EventGuest] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_EventGuest_CalendarEventId] FOREIGN KEY ([CalendarEventId]) REFERENCES [CalendarEvent]([Id]),

);
GO

CREATE INDEX IX_EventGuest_CalendarEventId ON  [EventGuest] ([CalendarEventId])
INCLUDE ([Id], [GuestName], [GuestJobTitle]);
GO
