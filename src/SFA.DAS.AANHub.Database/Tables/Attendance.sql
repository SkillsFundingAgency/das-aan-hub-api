CREATE TABLE [dbo].[Attendance]
(
	[Id] BIGINT NOT NULL IDENTITY(1, 1) PRIMARY KEY, 
    [CalendarEventId] UNIQUEIDENTIFIER NOT NULL, 
    [MemberId] UNIQUEIDENTIFIER NOT NULL, 
    [ShowProfile] BIT NOT NULL, 
    [NotifyEmail] BIT NOT NULL, 
    [NotifySMS] BIT NOT NULL, 
    [NotifyInService] BIT NOT NULL, 
    [Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [Updated] DATETIME NULL, 
    [Deleted] DATETIME NULL, 
    [IsActive] BIT NOT NULL, 
    [Attended] BIT NOT NULL, 
    [Notes] NCHAR(200) NOT NULL, 
    [ContactsMade] INT NOT NULL
    CONSTRAINT [FK_Attendance_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id])
    CONSTRAINT [FK_Attendance_CalendarEvent] FOREIGN KEY ([CalendarEventId]) REFERENCES [CalendarEvent]([Id])
)
