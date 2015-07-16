select
	TeamName as [Team Name],
	CountryCode as [Country Code]
from Teams
where TeamName LIKE '%[0-9]%'