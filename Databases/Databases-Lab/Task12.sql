select 
	c.Name as [Category],
	u.Username,
	u.PhoneNumber,
	count(a.Id) as [Answers Count]
from Categories c
left join Questions q
	on c.Id = q.CategoryId
left join Answers a
	on a.QuestionId = q.Id
left join Users u
	on u.Id = a.UserId
where u.PhoneNumber is not null
group by c.Name, u.Username, u.PhoneNumber
order by [Answers Count] desc, u.Username