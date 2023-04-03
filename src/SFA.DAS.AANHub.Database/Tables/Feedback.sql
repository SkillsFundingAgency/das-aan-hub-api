CREATE TABLE [dbo].[Feedback]
(
    [AttendanceId] UNIQUEIDENTIFIER FOREIGN KEY REFERENCES [dbo].[Attendance](Id),
    [AttributeId] INT NOT NULL, 
    [AttributeValue] NVARCHAR(max) NOT NULL
    CONSTRAINT PK_Feedback PRIMARY KEY (AttendanceId, AttributeId)
)