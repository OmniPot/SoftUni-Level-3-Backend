--For each continent, display the total area and total population of all its countries. 
--Sort the results by population from highest to lowest. Submit for evaluation the result grid with headers.

select 
	ContinentName,
	SUM(cou.AreaInSqKm) as [CountriesArea],
	SUM(CAST(cou.Population as bigint)) as [CountriesPopulation]
from Continents con
join Countries cou
	on cou.ContinentCode = con.ContinentCode
group by con.ContinentName
order by [CountriesPopulation] desc