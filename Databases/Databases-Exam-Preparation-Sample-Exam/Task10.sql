select
	a.Id,
	a.Title,
	a.Date,
	s.Status
from Ads a
join AdStatuses s
on s.Id = a.StatusId
where 
datepart(M, a.Date) = (select month(min(Date)) from Ads) and
datepart(year, a.Date) = (select year(min(Date)) from Ads) and
s.Status != 'Published'
order by a.Id