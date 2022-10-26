CREATE TABLE [dbo].[MemberProfile]
(
	[MemberId] VARCHAR(50) NOT NULL PRIMARY KEY, 
    [ProfileId] NVARCHAR(MAX) NOT NULL, 
    [ProfileValue] NVARCHAR(MAX) NOT NULL
)
