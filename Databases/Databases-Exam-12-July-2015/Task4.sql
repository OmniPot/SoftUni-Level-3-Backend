select
	Username as [Username],
	IpAddress as [IP Address]
from Users
where IpAddress like '___.1%.%.___'
order by Username 