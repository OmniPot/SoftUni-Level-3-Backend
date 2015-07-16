select
	(select distinct LeagueName from Leagues where Id = l.Id) as [League Name],
	(select count(lt.TeamId) from Teams t 
		join Leagues_Teams lt on t.Id = lt.TeamId and lt.LeagueId = l.Id) as [Teams],
	(select count(Id) from TeamMatches where l.Id = LeagueId) as [Matches],
	(select isnull(avg(tm.HomeGoals + tm.AwayGoals), 0) from TeamMatches tm where tm.LeagueId = l.Id) as [Average Goals]
from Leagues l
order by [Teams] desc, [Matches] desc