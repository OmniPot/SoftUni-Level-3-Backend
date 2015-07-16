CREATE VIEW LoggedToday AS
SELECT UserName, FullName
FROM Users 
WHERE 
	YEAR(LastLogin) = YEAR(GETDATE()) AND
	MONTH(LastLogin) = MONTH(GETDATE()) AND
	DAY(LastLogin) = DAY(GETDATE())
