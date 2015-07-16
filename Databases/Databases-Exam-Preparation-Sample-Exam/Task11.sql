select
	s.Status,
	count(*) as [Count]
from AdStatuses s
join Ads a
	on a.StatusId = s.Id
group by s.Status