

IF EXISTS(SELECT * FROM sys.tables WHERE Name = 'Feedback')
  DROP TABLE Feedback;
GO