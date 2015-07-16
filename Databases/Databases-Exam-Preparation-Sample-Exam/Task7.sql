select
	a.Title,
	c.Name as CategoryName,
	t.Name as TownName,
	s.Status
from Ads a
join Categories c
	on c.Id = a.CategoryId
join Towns t
	on t.Id = a.TownId
join AdStatuses s
	on s.Id = a.StatusId
where t.Name in ('Sofia', 'Blagoevgrad', 'Stara Zagora') and s.Status = 'Published'
order by Title