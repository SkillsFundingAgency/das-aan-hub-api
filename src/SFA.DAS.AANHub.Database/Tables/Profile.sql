CREATE TABLE [dbo].[Profile]
(
    [Id] INT NOT NULL,
    [UserType] NVARCHAR(20) NOT NULL ,
    [Category] NVARCHAR(20) NOT NULL, 
    [Description] NVARCHAR(200) NOT NULL, 
    [Ordering] INT NOT NULL,
    [PreferenceId] INT NULL,
    CONSTRAINT [PK_Profile] PRIMARY KEY ([Id])
)
