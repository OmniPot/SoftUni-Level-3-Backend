USE Bank
GO

CREATE PROC usp_SelectPersonsWithCertainBalance(@minBalance money = 0)
AS
	SELECT p.FirstName + ' ' + p.LastName as Person, a.Balance
	FROM Accounts a
		JOIN Persons p
	ON a.PersonId = p.Id
	WHERE a.Balance > @minBalance
GO

EXEC usp_selectPersonsWithCertainBalance 4000
