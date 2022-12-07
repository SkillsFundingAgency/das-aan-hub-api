CREATE TABLE [dbo].[CalendarPermission]
(
    [CalendarId] BIGINT NOT NULL PRIMARY KEY,
    [PermissionId] BIGINT NOT NULL,
    [HasCreate] BIT NOT NULL,
    [HasUpdate] BIT NOT NULL,
    [HasView] BIT NOT NULL DEFAULT 1,
    [HasDelete] BIT NOT NULL
    CONSTRAINT [FK_CalendarPermission_Calendar] FOREIGN KEY ([CalendarId]) REFERENCES [Calendar]([Id])
    CONSTRAINT [FK_CalendarPermission_Permission] FOREIGN KEY ([PermissionId]) REFERENCES [Permission]([Id])
)