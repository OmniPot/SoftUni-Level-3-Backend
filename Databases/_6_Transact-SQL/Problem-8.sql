USE SoftUni
GO

DECLARE empCursor CURSOR READ_ONLY FOR
	SELECT 
		e1.FirstName + ' ' + e1.LastName as Emp1Name,
		t2.Name,
		e2.FirstName + ' ' + e2.LastName as Emp2Name
	FROM Employees e1
	CROSS JOIN Employees e2
	JOIN Addresses a1
		ON e1.AddressID = a1.AddressID
	JOIN Addresses a2
		ON e2.AddressID = a2.AddressID
	JOIN Towns t1
		ON a1.TownID = t1.TownID
	JOIN Towns t2
		ON a2.TownID = t2.TownID
	WHERE t1.TownID = t2.TownID AND e1.EmployeeID != e2.EmployeeID

OPEN empCursor
DECLARE @name1 nvarchar(30), @townName nvarchar(30), @name2 nvarchar(30)
FETCH NEXT FROM empCursor INTO @name1, @townName, @name2

WHILE @@FETCH_STATUS = 0
	BEGIN
		PRINT @name1 + ' ' + @townName + ' ' + @name2
		FETCH NEXT FROM empCursor INTO @name1, @townName, @name2
	END

CLOSE empCursor
DEALLOCATE empCursor
