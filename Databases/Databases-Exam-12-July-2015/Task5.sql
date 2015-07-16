select
	g.Name as [Game],
	(case when datepart(hour, g.Start) >= 0 and datepart(hour, g.Start) < 12 then 'Morning'
		when datepart(hour, g.Start) >= 12 and datepart(hour, g.Start) < 18 then 'Afternoon'
		when datepart(hour, g.Start) >= 18 and datepart(hour, g.Start) < 24 then 'Evening' end) as [Part of the Day],
	(case when g.Duration <= 3 then 'Extra Short'
		when g.Duration >= 4 and g.Duration <= 6 then 'Short'
		when g.Duration > 6 then 'Long'	else 'Extra Long' end) as [Duration]
from Games g
order by g.Name, [Duration], [Part of the Day]