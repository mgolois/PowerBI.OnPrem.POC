CREATE TABLE [dbo].[SurveyResults]
(
	[ID] int NOT NULL PRIMARY KEY,
	FirstName VARCHAR(50),
	LastName VARCHAR(50),
	Email VARCHAR(50),
	Race VARCHAR(50),
	Language VARCHAR(50),
	Gender VARCHAR(50),
	Company VARCHAR(50),
	Salary money,
	JobTitle VARCHAR(50),
	Latitude decimal(9,6),
	Longitude decimal(9,6)
)
