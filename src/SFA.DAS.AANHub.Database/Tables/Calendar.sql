CREATE TABLE [dbo].[Calendar]
(
    [Id] BIGINT NOT NULL IDENTITY(1, 1) PRIMARY KEY, 
    [CalendarName] NVARCHAR(200) NOT NULL, 
    [EffectiveFrom] DATE NOT NULL, 
    [EffectiveTo] DATE NULL, 
    [Ordering] INT NOT NULL
)
