select
	t.Name as [Town Name],
	s.Status,
	count(*) as [Count]
from Towns t
join Ads a
	on a.TownId = t.Id
join AdStatuses s
	on a.StatusId = s.Id
group by t.Name, s.Status
order by t.Name, s.Status