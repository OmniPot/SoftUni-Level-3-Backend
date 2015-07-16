--Find the 30 biggest countries by population from Europe.
--Display the country name and population. Sort the results by population (from biggest to smallest), 
--then by country alphabetically. Submit for evaluation the result grid with headers.

select top 30
	CountryName,
	Population
from Countries country
join Continents continent
	on continent.ContinentCode = country.ContinentCode
where continent.ContinentName = 'Europe'
order by Population desc, CountryName