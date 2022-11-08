CREATE TABLE [dbo].[MemberProfile]
(
	[MemberId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [ProfileId] BIGINT NOT NULL, 
    [ProfileValue] NVARCHAR(200) NOT NULL
)
