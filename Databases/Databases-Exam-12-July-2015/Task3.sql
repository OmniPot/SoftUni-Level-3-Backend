select
	u.Username as [Username],
	substring(u.Email, charindex('@', u.Email) + 1, len(u.Email) - charindex('@', u.Email) + 1) as [Email Provider]
from Users u
order by [Email Provider], u.Username