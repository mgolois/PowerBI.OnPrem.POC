CREATE TABLE [dbo].[Users](
	[UserId] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Username] [nchar](50) NULL,
	[Password] [nchar](100) NULL,
	[IsAdmin] [bit] NULL
	)


