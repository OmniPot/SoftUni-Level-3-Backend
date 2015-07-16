select
	Username,
	LastName,
	(case when PhoneNumber is null then '0' else '1' end) as [Has Phone]
from Users
order by LastName, Id