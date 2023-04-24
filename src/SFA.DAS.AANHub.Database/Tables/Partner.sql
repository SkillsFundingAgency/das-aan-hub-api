CREATE TABLE [dbo].[Partner]
(
    [MemberId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [UserName] NVARCHAR(200) NOT NULL, 
    CONSTRAINT [FK_Partner_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id]),
    CONSTRAINT [UK_Partner] UNIQUE ([UserName])
)
