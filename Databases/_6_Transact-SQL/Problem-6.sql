USE Bank
GO

CREATE TABLE Logs(
	Id int IDENTITY,
	AccountId int NOT NULL,
	OldSum money NOT NULL,
	NewSum money NOT NULL,
	CONSTRAINT PK_LogId PRIMARY KEY(Id),
	CONSTRAINT FK_Logs_Accounts 
		FOREIGN KEY(AccountId)
		REFERENCES Accounts(Id)
)
GO

CREATE TRIGGER tr_AccountsUpdate ON Accounts FOR UPDATE
AS
	DECLARE @accountId int = (SELECT Id FROM INSERTED)
	DECLARE @oldSum money = (SELECT Balance FROM DELETED)
	DECLARE @newSum money = (SELECT Balance FROM INSERTED)

	INSERT INTO Logs(AccountId, OldSum, NewSum) VALUES(@accountId, @oldSum, @newSum)
GO

UPDATE Accounts
SET Balance = 8526.23
WHERE Id = 3

SELECT Balance FROM Accounts WHERE Id = 3
GO