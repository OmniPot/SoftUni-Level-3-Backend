CREATE TABLE Groups (
	Id INT IDENTITY,
	Name NVARCHAR(30) UNIQUE,
	CONSTRAINT PK_Groups PRIMARY KEY(Id)
)