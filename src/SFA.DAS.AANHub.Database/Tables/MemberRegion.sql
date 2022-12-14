CREATE TABLE [dbo].[MemberRegion]
(
	[MemberId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [RegionId] INT NOT NULL
	CONSTRAINT [FK_MemberRegion_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id])
	CONSTRAINT [FK_MemberRegion_Region] FOREIGN KEY ([RegionId]) REFERENCES [Region]([Id])
)
