CREATE TABLE [dbo].[Attendance]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
    [CalendarEventId] UNIQUEIDENTIFIER NOT NULL, 
    [MemberId] UNIQUEIDENTIFIER NOT NULL,
    [AddedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [IsAttending] BIT NOT NULL DEFAULT 1, 
    CONSTRAINT [FK_Attendance_CalendarEventId] FOREIGN KEY ([CalendarEventId]) REFERENCES [CalendarEvent]([Id]), 
    CONSTRAINT [FK_Attendance_MemberId] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id]), 
)
GO


CREATE UNIQUE INDEX IXU_Attendance_CalendarEventId_MemberId ON [dbo].[Attendance] ([CalendarEventId], [MemberId])
INCLUDE ([Id], [AddedDate], [IsAttending]);
GO

