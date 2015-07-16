select 
	t.TeamName,
	isnull((select sum(HomeGoals) from TeamMatches where t.Id = HomeTeamId), 0) + 
	isnull((select sum(AwayGoals) from TeamMatches where t.Id = AwayTeamId), 0) as [Total Goals]
from Teams t
order by [Total Goals] desc, t.TeamName