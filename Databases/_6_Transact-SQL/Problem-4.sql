USE Bank
GO

CREATE PROC usp_GiveInterestToAccount(@accountId int, @interestRate float)
AS
	DECLARE @newBalance money
	DECLARE @oldBalance money = (
		SELECT Balance
		FROM Accounts
		WHERE Id = @accountId)

	EXEC @newBalance = fn_CalculateInterest
		@sum = @oldBalance,
		@interestRate = @interestRate,
		@months = 1;
	UPDATE Accounts
	SET Balance = @newBalance
	WHERE Id = @accountId
GO

DECLARE @accountIdToUpdate int = 1
DECLARE @interestRate float = 5.0
EXEC usp_GiveInterestToAccount
	@accountIdToUpdate,
	@interestRate;

SELECT Balance FROM Accounts WHERE Id = @accountIdToUpdate
GO