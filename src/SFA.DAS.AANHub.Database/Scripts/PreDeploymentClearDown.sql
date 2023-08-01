IF EXISTS(SELECT * FROM sys.tables WHERE Name = 'AttendanceProfile')
  DROP TABLE AttendanceProfile;
GO

IF EXISTS(SELECT * FROM sys.tables WHERE Name = 'CalendarPermission')
  DROP TABLE CalendarPermission;
GO

IF EXISTS(SELECT * FROM sys.tables WHERE Name = 'MemberPermission')
  DROP TABLE MemberPermission;
GO

IF EXISTS(SELECT * FROM sys.tables WHERE Name = 'Permission')
  DROP TABLE Permission;
GO

IF EXISTS(SELECT * FROM sys.tables WHERE Name = 'MemberRegion')
  DROP TABLE MemberRegion;
GO

IF EXISTS(SELECT * FROM sys.tables WHERE Name = 'Documents')
  DROP TABLE Documents;
GO

IF EXISTS(SELECT * FROM sys.tables WHERE Name = 'Notifications')
  DROP TABLE Notifications;
GO

IF EXISTS(SELECT * FROM sys.tables WHERE Name = 'Admin')
  DROP TABLE Admin;
GO

