select
	a.Content as [Answer Content],
	q.Content as [Question],
	c.Name as [Category]
from Answers a
join Questions q
	on q.Id = a.QuestionId
join Categories c
	on c.Id = q.CategoryId

order by c.Name