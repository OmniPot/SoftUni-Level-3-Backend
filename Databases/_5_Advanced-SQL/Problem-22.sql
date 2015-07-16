INSERT INTO Users(UserName, Password, FullName, LastLogin, GroupId)
SELECT 
	LOWER(SUBSTRING(FirstName, 1, 5)) + LOWER(SUBSTRING(LastName, 1, 5)),
	LOWER(SUBSTRING(FirstName, 1, 5)) + LOWER(SUBSTRING(LastName, 1, 5)),
	FirstName + ' ' + LastName,
	NULL,
	NULL
FROM Employees
