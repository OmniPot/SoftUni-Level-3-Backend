select 
	homeCou.CountryName as [Home Team],
	awayCou.CountryName as [Away Team],
	im.MatchDate as [Match Date]
from InternationalMatches im
join Countries homeCou
	on homeCou.CountryCode = im.HomeCountryCode
join Countries awayCou
	on awayCou.CountryCode = im.AwayCountryCode
order by im.MatchDate desc