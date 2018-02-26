CREATE TABLE [dbo].[Users](
	[UserId] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Username] NVARCHAR(50) NULL,
	[Password] NVARCHAR(100) NULL,
	[IsAdmin] [bit] NULL
	)


