select
	u.UserName,
	count(a.Id) as [AdsCount],
	(case when r.Name = 'Administrator' then 'yes' else 'no' end) as [IsAdministrator]
from AspNetUsers u
left join Ads a
	on a.OwnerId = u.Id
left join AspNetUserRoles ur
	on ur.UserId = u.Id
left join AspNetRoles r
	on r.Id = ur.RoleId and r.Name = 'Administrator'
group by u.UserName, r.Name
order by u.UserName