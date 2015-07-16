USE Bank
GO

CREATE FUNCTION fn_CalculateInterest(
	@sum MONEY,
	@interestRate FLOAT,
	@months INT
) RETURNS MONEY
AS
BEGIN
	DECLARE @newSum money = @sum * (1 + ((@interestRate * @months) / 100))
	RETURN @newSum
END
GO

DECLARE @newSum money
EXEC @newSum = fn_CalculateInterest 
	@sum = 1000, 
	@interestRate = 1.8,
	@months = 24;

SELECT @newSum as [NewSum]
GO