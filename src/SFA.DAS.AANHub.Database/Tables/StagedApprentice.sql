﻿CREATE TABLE [dbo].[StagedApprentice](
	[Id] INT IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](200) NOT NULL,
	[LastName] [nvarchar](200) NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[DateOfBirth] [datetime2](7) NOT NULL,
	[Uln] [bigint] NULL,
	[ApprenticeshipId] [bigint] NULL,
	[EmployerName] [nvarchar](200) NULL,
	[StartDate] [datetime2](7) NOT NULL,
	[EndDate] [datetime2](7) NULL,
	[TrainingProviderId] [bigint] NULL,
	[TrainingProviderName] [nvarchar](200) NULL,
	[TrainingCode] [nvarchar](15) NULL,
	[TrainingCourseOption] [nvarchar](126) NULL,
	[StandardUId] [nvarchar](20) NULL,
	[CreatedOn] [datetime2](7) NOT NULL DEFAULT (getutcdate()),
	CONSTRAINT PK_StagedApprentice_Id PRIMARY KEY (Id),
	CONSTRAINT UK_StagedApprentice_Email_LastName_DateOfBirth UNIQUE ([Email], [LastName], [DateOfBirth])
)