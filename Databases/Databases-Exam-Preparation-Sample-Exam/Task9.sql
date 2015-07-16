select top 10
	Title,
	Date,
	Status
from Ads a
join AdStatuses s
	on s.Id = a.StatusId
order by Date desc