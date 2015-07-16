select
	min(CreatedOn) as [MinDate],
	max(CreatedOn) as [MaxDate]
from Answers a
where a.CreatedOn between '01-01-2012' and '12-31-2014'