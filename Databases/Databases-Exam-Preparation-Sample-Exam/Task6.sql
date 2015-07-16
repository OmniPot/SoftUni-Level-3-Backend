select
	a.Title,
	c.Name as [CategoryName],
	t.Name as [TownName],
	s.Status
from Ads a
left join Categories c
	on c.Id = a.CategoryId
left join Towns t
	on t.Id = a.TownId
left join AdStatuses s
	on s.Id = a.StatusId