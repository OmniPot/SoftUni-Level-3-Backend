--Find all countries that holds the letter 'A' in their name at least 3 times (case insensitively), 
--sorted by ISO code. Display the country name and ISO code. Submit for evaluation the result grid with headers.

select 
	CountryName as [Country Name],
	IsoCode as [ISO Code]
from Countries
where CountryName LIKE '%A%A%A%'
order by IsoCode