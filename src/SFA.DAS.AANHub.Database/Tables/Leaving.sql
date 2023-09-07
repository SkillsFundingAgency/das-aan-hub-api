CREATE TABLE [dbo].[Leaving]
(
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(), 
    [MemberId] UNIQUEIDENTIFIER NOT NULL,
    [LeavingReasonId] INT NOT NULL, 
    CONSTRAINT [PK_Leaving] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_Leaving_Member] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id]),    
    CONSTRAINT [FK_Leaving_LeavingReasonId] FOREIGN KEY ([LeavingReasonId]) REFERENCES [LeavingReason]([Id])    
)
GO
