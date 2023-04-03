CREATE TABLE [dbo].[FeedbackAttribute]
(
    [Id] INT NOT NULL PRIMARY KEY,
    [AttributeName] NVARCHAR(100) NOT NULL,
    [Category] NVARCHAR(100) NOT NULL,
    [EventFormat] NVARCHAR(10) NOT NULL,
    [Ordering] INT NOT NULL DEFAULT 0
)
