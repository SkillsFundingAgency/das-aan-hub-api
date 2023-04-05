CREATE TABLE [dbo].[Calendar]
(
    [Id] INT NOT NULL PRIMARY KEY, 
    [CalendarName] NVARCHAR(200) NOT NULL, 
    [EffectiveFrom] DATE NOT NULL, 
    [EffectiveTo] DATE NULL, 
    [Ordering] INT NOT NULL
)
