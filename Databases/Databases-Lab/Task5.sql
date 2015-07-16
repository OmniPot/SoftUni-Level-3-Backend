select top 50
	a.Content as [Answer Content],
	a.CreatedOn,
	u.Username as [Answer Author],
	q.Title as [Question Title],
	c.Name as [Category Name]
from Answers a
join Questions q
	on a.QuestionId = q.Id
join Users u
	on u.Id = a.UserId
join Categories c
	on c.Id = q.CategoryId
order by c.Name, u.Username, a.CreatedOn