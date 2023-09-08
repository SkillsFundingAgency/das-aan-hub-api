
IF EXISTS(SELECT * FROM sys.tables WHERE Name = 'Partner')
  DROP TABLE Partner;
GO