/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

:r .\AddRegionData.sql
:r .\AddProfileData.sql
:r .\AddPreferenceData.sql
:r .\AddCalendarData.sql
:r .\AddNotificationTemplateData.sql
:r .\AddLeavingReasonData.sql
:r .\UpdateCalendarEventLastUpdatedDate.sql
:r .\UpdateEntityIdInAuditForCalendarEvents.sql

