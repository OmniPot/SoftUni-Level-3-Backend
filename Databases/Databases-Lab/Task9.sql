select top 10
	a.Content as [Answer],
	a.CreatedOn,
	u.Username
from Answers a
join Users u
	on a.UserId = u.Id
order by a.CreatedOn desc