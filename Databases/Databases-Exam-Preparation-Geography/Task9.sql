--For each country in the database, display the number of rivers passing through that country 
--and the total length of these rivers. When a country does not have any river, display 0 as rivers count 
--and as total length. Sort the results by rivers count (from largest to smallest), then by total length (from largest to smallest),
--then by country alphabetically. Submit for evaluation the result grid with headers.

select 
	coun.CountryName,
	con.ContinentName,
	count(r.Id) as [RiversCount],
	isnull(sum(r.Length), 0) as [TotalLength]
from Countries coun
left join Continents con
	on coun.ContinentCode = con.ContinentCode
left join CountriesRivers cr
	on coun.CountryCode = cr.CountryCode
left join Rivers r
	on r.Id = cr.RiverId
group by con.ContinentName, coun.CountryName
order by [RiversCount] desc, [TotalLength] desc, coun.CountryName