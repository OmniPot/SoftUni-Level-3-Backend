select 
	t.TeamName as [Team Name],
	l.LeagueName as [League],
	isNull(cou.CountryName, 'International') as [League Country]
from Teams t
join Leagues_Teams lt
	on lt.TeamId = t.Id
join Leagues l
	on l.Id = lt.LeagueId
left join Countries cou
	on l.CountryCode= cou.CountryCode
order by t.TeamName, l.LeagueName