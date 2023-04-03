CREATE TABLE [dbo].[Attendance]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
    [CalendarEventId] UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES [CalendarEvent]([Id]), 
    [MemberId] UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES [Member]([Id]),
    [Added] DATETIME2 DEFAULT GETUTCDATE(),
    [IsActive] BIT NOT NULL DEFAULT 1, 
    [Attended] BIT NOT NULL DEFAULT 0, 
    [OverallRating] INT NULL, 
    [FeedbackCompleted] DATETIME2 NULL
)
GO

CREATE UNIQUE INDEX IXU_Attendance_CalendarEventId_MemberId ON [dbo].[Attendance] ([CalendarEventId], [MemberId])
INCLUDE ([Id], [Added], [IsActive], [Attended], [OverallRating], [FeedbackCompleted]);
GO
