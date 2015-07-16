USE Bank;
GO

CREATE PROC usp_WithdrawMoney(@accountId int, @amountOfMoney money = 0)
AS
	BEGIN TRANSACTION

	DECLARE @currentBalance money = (SELECT Balance FROM Accounts WHERE Id = @accountId)
	IF (@currentBalance - @amountOfMoney > 0)
		BEGIN
			UPDATE Accounts
			SET Balance = Balance - @amountOfMoney
			WHERE Id = @accountId
		END
	ELSE
		BEGIN
			PRINT 'Not enough money in the bank account.'
		END

	COMMIT TRANSACTION
	GO
GO

CREATE PROC usp_DepositMoney(@accountId int, @amountOfMoney money = 0)
AS
	BEGIN TRANSACTION

	UPDATE Accounts
	SET Balance = Balance + @amountOfMoney
	WHERE Id = @accountId

	COMMIT TRANSACTION
	GO
GO

DECLARE @accountId int = 3
DECLARE @amount money = 5000
EXEC usp_DepositMoney
	@accountId,
	@amount;

SELECT Balance FROM Accounts WHERE Id = @accountId
GO

DECLARE @accountId int = 6
DECLARE @amount money = 20000
EXEC usp_WithdrawMoney
	@accountId,
	@amount;

SELECT Balance FROM Accounts WHERE Id = @accountId
GO