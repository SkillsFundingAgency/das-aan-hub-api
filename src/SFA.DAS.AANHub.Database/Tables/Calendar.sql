CREATE TABLE [dbo].[Calendar]
(
    [Id] INT NOT NULL PRIMARY KEY, 
    [CalendarName] NVARCHAR(200) NOT NULL, 
    [EffectiveFromDate] DATE NOT NULL, 
    [EffectiveToDate] DATE NULL, 
    [Ordering] INT NOT NULL
)
