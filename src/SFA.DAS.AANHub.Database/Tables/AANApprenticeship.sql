CREATE TABLE [dbo].[AANApprenticeship](
	[Id] INT IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](200) NOT NULL,
	[LastName] [nvarchar](200) NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[DateOfBirth] [datetime2](7) NOT NULL,
	[Uln] [bigint] NULL,
	[ApprenticeshipId] [bigint] NULL,
	[StartDate] [datetime2](7) NOT NULL,
	[EndDate] [datetime2](7) NULL,
	[TrainingProviderId] [bigint] NULL,
	[TrainingProviderName] [nvarchar](200) NULL,
	[TrainingCode] [nvarchar](15) NOT NULL,
	[TrainingCourseOption] [nvarchar](126) NULL,
	[StandardUId] [nvarchar](20) NULL,
	[CreatedOn] [datetime2](7) NOT NULL DEFAULT (getutcdate()),
	CONSTRAINT PK_AANApprenticeship_Id PRIMARY KEY (Id),
	CONSTRAINT UK_AANApprenticeship_Email_Lastname_DateOfBirth UNIQUE ([Email], [Lastname], [DateOfBirth])
)