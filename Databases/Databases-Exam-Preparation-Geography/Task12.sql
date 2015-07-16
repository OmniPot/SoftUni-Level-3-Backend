--For each country, find the elevation of the highest peak and the length of the longest river,
--sorted by the highest peak elevation (from highest to lowest),
--then by the longest river length (from longest to smallest),
--then by country name (alphabetically). Display NULL when no data is available in some of the columns. 
--Submit for evaluation the result grid with headers.

select 
	CountryName,
	MAX(p.Elevation) as [HighestPeakElevation],
	MAX(r.Length) as [LongestRiverLength]
from Countries cou
left join MountainsCountries mc
	on mc.CountryCode = cou.CountryCode
left join Mountains m
	on m.Id = mc.MountainId
left join Peaks p
	on p.MountainId = m.Id
left join CountriesRivers cr
	on cr.CountryCode = cou.CountryCode
left join Rivers r
	on r.Id = cr.RiverId
group by cou.CountryName
order by HighestPeakElevation desc, LongestRiverLength desc, cou.CountryName