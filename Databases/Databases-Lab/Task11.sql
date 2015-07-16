select
	c.Name as [Category],
	count(a.Id) as [Answers Count]
from Categories c
left join Questions q
	on q.CategoryId = c.Id
left join Answers a
	on a.QuestionId = q.Id
group by c.Name
order by [Answers Count] desc