select 
	t.TeamName as [Team],
	count(*) as [Matches Count]
from Teams t
join TeamMatches tm
	on t.Id = tm.AwayTeamId OR t.Id = tm.HomeTeamId
group by t.TeamName
having count(*) > 1
order by t.TeamName