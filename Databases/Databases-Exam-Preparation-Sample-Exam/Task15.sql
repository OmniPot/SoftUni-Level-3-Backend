select
	a1.Date as [FirstDate],
	a2.Date as [SecondDate]
from Ads a1, Ads a2
where a2.Date > a1.Date and datediff(hour, a1.date, a2.Date) < 12
order by a1.Date, a2.Date