USE SoftUni
GO

CREATE FUNCTION ufn_WordConsistsOfGivenLetters(@symbols nvarchar(30), @word nvarchar(30)) RETURNS nvarchar
AS
BEGIN
	DECLARE @wordIndex int = 1
	DECLARE @wordSymbolToCheck nvarchar(1)

	WHILE(@wordIndex <= LEN(@word))
		BEGIN
			SET @wordSymbolToCheck = SUBSTRING(@word, @wordIndex, 1)

			IF (@symbols NOT LIKE '%' + @wordSymbolToCheck + '%')
				BEGIN
					RETURN NULL
				END
			ELSE
				BEGIN
					SET @wordIndex = @wordIndex + 1
				END
		END
	RETURN @word
END
GO

SELECT FirstName
FROM Employees e
WHERE 
	dbo.ufn_WordConsistsOfGivenLetters('abcdefghijklmnopq'' -.', e.FirstName) IS NOT NULL

UNION ALL

SELECT Name
FROM Towns t
WHERE
	dbo.ufn_WordConsistsOfGivenLetters('abcdefghijklmnopqrstuvwxyz'' -.', t.Name) IS NOT NULL