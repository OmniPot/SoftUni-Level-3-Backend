select 
	u.Id, 
	u.Username,
	u.FirstName,
	u.PhoneNumber, 
	u.RegistrationDate, 
	u.Email
from Users u
left join Questions q
	on u.Id = q.UserId
where u.PhoneNumber is null and q.Title is null
order by u.RegistrationDate