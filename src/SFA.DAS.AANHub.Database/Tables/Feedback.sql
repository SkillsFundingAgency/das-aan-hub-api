CREATE TABLE [dbo].[Feedback]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
    [AttendanceId] UNIQUEIDENTIFIER NOT NULL,
    [AttributeId] INT NOT NULL, 
    [AttributeValue] NVARCHAR(max) NOT NULL,
    CONSTRAINT [FK_Feedback_AttendanceId] FOREIGN KEY ([AttendanceId]) REFERENCES [Attendance]([Id]),     
    CONSTRAINT [FK_Feedback_AttributeId] FOREIGN KEY ([AttributeId]) REFERENCES [FeedbackAttribute]([Id])    
)
GO

CREATE UNIQUE INDEX IXU_Feedback_AttendanceId_AttributeId ON [Feedback]([AttendanceId], [AttributeId])
INCLUDE ([AttributeValue])
GO