--Find all peaks along with their mountain, country and continent.
--When a mountain belongs to multiple countries, display them all. Sort the results by peak name alphabetically,
--then by country name alphabetically. Submit for evaluation the result grid with headers.

select
	p.PeakName,
	m.MountainRange as Mountain,
	c.CountryName,
	con.ContinentName
from Peaks p
join Mountains m 
	on p.MountainId = m.Id
join MountainsCountries mc
	on mc.MountainId = m.Id
join Countries c
	on mc.CountryCode = c.CountryCode
join Continents con
	on c.ContinentCode = con.ContinentCode
order by p.PeakName, c.CountryName
