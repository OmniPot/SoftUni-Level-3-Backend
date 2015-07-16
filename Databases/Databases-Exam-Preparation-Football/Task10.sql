select
	tm.MatchDate as [First Date],
	stm.MatchDate as [Second Date]
from TeamMatches tm
join TeamMatches stm
	on datepart(dd, tm.MatchDate) = datepart(dd, stm.MatchDate)
where tm.Id != stm.Id and tm.MatchDate < stm.MatchDate
order by [First Date] desc, [Second Date] desc