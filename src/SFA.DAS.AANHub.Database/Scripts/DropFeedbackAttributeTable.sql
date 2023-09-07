
IF EXISTS(SELECT * FROM sys.tables WHERE Name = 'FeedbackAttribute')
  DROP TABLE FeedbackAttribute;
GO