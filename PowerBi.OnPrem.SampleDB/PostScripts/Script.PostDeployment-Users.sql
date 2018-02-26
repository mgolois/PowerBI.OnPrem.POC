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
Insert Users ([Username], [Password], [IsAdmin]) values ('admin', 'admin', 1)
Insert Users ([Username], [Password], [IsAdmin]) values ('golois', 'golois', 0)
Insert Users ([Username], [Password], [IsAdmin]) values ('michele', 'michele', 0)


Insert FolderAccess(FolderId, UserId, CanEdit, FolderName) values ('09a607e9-8bc5-475a-a904-2dd8451b9d90', 1, 1, 'Public')
Insert FolderAccess(FolderId, UserId, CanEdit, FolderName) values ('09a607e9-8bc5-475a-a904-2dd8451b9d90', 2, 1, 'Public')
Insert FolderAccess(FolderId, UserId, CanEdit, FolderName) values ('09a607e9-8bc5-475a-a904-2dd8451b9d90', 3, 0, 'Public')
Insert FolderAccess(FolderId, UserId, CanEdit, FolderName) values ('9cf21895-1dad-4343-b270-09b7efe99678', 3, 1, 'michele')