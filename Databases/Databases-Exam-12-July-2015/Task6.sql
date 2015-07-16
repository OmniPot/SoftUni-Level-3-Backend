select
	substring(u.Email, charindex('@', u.Email) + 1, len(u.Email) - charindex('@', u.Email) + 1) as [Email Provider],
	count(u.Id) as [Number Of Users]
from Users u
group by substring(u.Email, charindex('@', u.Email) + 1, len(u.Email) - charindex('@', u.Email) + 1)
order by [Number Of Users] desc, [Email Provider]