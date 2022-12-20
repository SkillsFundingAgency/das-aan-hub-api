CREATE TABLE [dbo].[MemberRegion]
(
	[MemberId] UNIQUEIDENTIFIER NOT NULL, 
    [RegionId] INT NOT NULL
	CONSTRAINT [PK_MemberRegion] PRIMARY KEY (MemberId, RegionId)
	CONSTRAINT [FK_MemberRegion_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id])
	CONSTRAINT [FK_MemberRegion_Region] FOREIGN KEY ([RegionId]) REFERENCES [Region]([Id])
)
