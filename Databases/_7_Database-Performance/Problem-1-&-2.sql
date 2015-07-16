USE SoftUni
GO

--Insert some data

DECLARE @daysToAdd int = 1
DECLARE @description nvarchar(MAX) = 'Beers need no description...'

WHILE (@daysToAdd < 1000000)
BEGIN
	INSERT INTO Beers(DrunkOn, Description)
	VALUES(DATEADD(dd, @daysToAdd, GETDATE()), @description)
	SET @daysToAdd = @daysToAdd + 1
END

--Select in date range

DBCC DROPCLEANBUFFERS;
DBCC FREEPROCCACHE;

SELECT DrunkOn, Description
FROM Beers
WHERE DrunkOn BETWEEN '2016-1-1' AND '4700-1-1'

--Add index to speed the search by date

CREATE NONCLUSTERED INDEX [IX_BeersDrunkOn] 
ON Beers(DrunkOn)

--Test the index

DBCC DROPCLEANBUFFERS;
DBCC FREEPROCCACHE;

SELECT DrunkOn
FROM Beers
WHERE DrunkOn BETWEEN '2016-1-1' AND '4700-1-1'