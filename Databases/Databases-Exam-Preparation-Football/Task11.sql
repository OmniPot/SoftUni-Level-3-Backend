select
	lower(t.TeamName + substring(reverse(t2.TeamName), 2, len(t2.TeamName))) as [Mix]
from Teams t, Teams t2
where substring(t.TeamName, len(t.TeamName), 1) = substring(reverse(t2.TeamName), 1, 1)
order by [Mix]