select
	c.CountryName as [Country Name],
	(
		select count(Id) 
		from InternationalMatches 
		where (c.CountryCode = HomeCountryCode or c.CountryCode = AwayCountryCode)
	) as [International Matches],
	(
		select count(tm.Id) 
		from TeamMatches tm 
		join Leagues l 
			on l.CountryCode = c.CountryCode and tm.LeagueId = l.Id
	) as [Team Matches]
from Countries c
where (
		select count(Id) 
		from InternationalMatches 
		where (c.CountryCode = HomeCountryCode or c.CountryCode = AwayCountryCode)
		having count(*) > 0
	) > 0 or (
		select count(tm.Id) 
		from TeamMatches tm 
		join Leagues l 
			on l.CountryCode = c.CountryCode and tm.LeagueId = l.Id
	) > 0
order by [International Matches] desc, [Team Matches] desc, c.CountryName