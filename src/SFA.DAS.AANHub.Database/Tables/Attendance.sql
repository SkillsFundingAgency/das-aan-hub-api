CREATE TABLE [dbo].[Attendance]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
    [CalendarEventId] UNIQUEIDENTIFIER NOT NULL, 
    [MemberId] UNIQUEIDENTIFIER NOT NULL,
    [AddedDate] DATETIME2 DEFAULT GETUTCDATE(),
    [IsActive] BIT NOT NULL DEFAULT 1, 
    [Attended] BIT NOT NULL DEFAULT 0, 
    [OverallRating] INT NULL, 
    [FeedbackCompletedDate] DATETIME2 NULL,
    CONSTRAINT [FK_Attendance_CalendarEventId] FOREIGN KEY ([CalendarEventId]) REFERENCES [CalendarEvent]([Id]), 
    CONSTRAINT [FK_Attendance_MemberId] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id]), 
)
GO


CREATE UNIQUE INDEX IXU_Attendance_CalendarEventId_MemberId ON [dbo].[Attendance] ([CalendarEventId], [MemberId])
INCLUDE ([Id], [AddedDate], [IsActive], [Attended], [OverallRating], [FeedbackCompletedDate]);
GO

