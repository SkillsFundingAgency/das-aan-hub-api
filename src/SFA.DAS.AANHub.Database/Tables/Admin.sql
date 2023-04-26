CREATE TABLE [dbo].[Admin]
(
    [MemberId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [UserName] NVARCHAR(200) NOT NULL,
    CONSTRAINT [FK_Admin_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id]),
    CONSTRAINT [UK_Admin] UNIQUE ([UserName])
)
