select
	c.Name as [Category],
	q.Title as [Question],
	q.CreatedOn
from Categories c
left join Questions q
	on q.CategoryId = c.Id
order by c.Name, q.Title