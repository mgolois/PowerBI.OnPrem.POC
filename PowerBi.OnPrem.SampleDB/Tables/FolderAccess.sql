CREATE TABLE [dbo].[FolderAccess]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [FolderId] UNIQUEIDENTIFIER NOT NULL, 
    [UserId] int NOT NULL, 
    CONSTRAINT [FK_FolderAccess_ToTable] FOREIGN KEY (UserId) REFERENCES Users(UserId)
)
