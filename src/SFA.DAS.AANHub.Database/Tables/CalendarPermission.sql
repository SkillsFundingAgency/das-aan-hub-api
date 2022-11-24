CREATE TABLE [dbo].[CalendarPermission]
(
    [CalendarId] BIGINT NOT NULL PRIMARY KEY,
    [PermissionId] BIGINT NOT NULL,
    [Create] BIT NOT NULL,
    [Update] BIT NOT NULL,
    [View] BIT NOT NULL DEFAULT 1,
    [Delete] BIT NOT NULL
    CONSTRAINT [FK_CalendarPermission_Calendar] FOREIGN KEY ([CalendarId]) REFERENCES [Calendar]([Id])
    CONSTRAINT [FK_CalendarPermission_Permission] FOREIGN KEY ([PermissionId]) REFERENCES [Permission]([Id])
)