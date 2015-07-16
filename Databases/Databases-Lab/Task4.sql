select 
	q.Title as [Question Title],
	u.Username as [Author]
from Questions q
join Users u
	on q.UserId = u.Id
order by q.Title asc