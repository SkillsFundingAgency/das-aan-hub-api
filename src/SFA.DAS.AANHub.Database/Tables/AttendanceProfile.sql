CREATE TABLE [dbo].[AttendanceProfile]
(
	[AttendanceId] BIGINT NOT NULL PRIMARY KEY, 
    [MemberId] BIGINT NOT NULL, 
    [ProfileId] BIGINT NOT NULL, 
    [Allowed] BIT NOT NULL
    CONSTRAINT [FK_AttendanceProfile_Attendance] FOREIGN KEY ([AttendanceId]) REFERENCES [Attendance]([Id])
    CONSTRAINT [FK_AttendanceProfile_Profile] FOREIGN KEY ([ProfileId]) REFERENCES [Profile]([Id])
)
