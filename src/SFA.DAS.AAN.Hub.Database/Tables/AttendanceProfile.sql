CREATE TABLE [dbo].[AttendanceProfile]
(
	[AttendanceId] BIGINT NOT NULL PRIMARY KEY, 
    [MemberId] BIGINT NOT NULL, 
    [ProfileId] BIGINT NOT NULL, 
    [Allowed] BIT NOT NULL
)
