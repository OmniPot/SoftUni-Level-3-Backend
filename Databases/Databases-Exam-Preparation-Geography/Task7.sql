--Find all rivers that pass through to 3 or more countries. Display the river name and the number of countries.
--Sort the results by river name. Submit for evaluation the result grid with headers.

select 
	RiverName as [River],
	count(cr.CountryCode) as [Countries Count]
from Rivers r 
join CountriesRivers cr
	on cr.RiverId = r.Id
group by RiverName
having COUNT(CountryCode) >= 3
order by RiverName