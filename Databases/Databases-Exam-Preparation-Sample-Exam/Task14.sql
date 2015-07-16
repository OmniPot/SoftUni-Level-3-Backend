select
	count(a.Id) as [AdsCount],
	isnull(t.Name, '(no town)') as [Town]
from Ads a
left join Towns t
	on a.TownId = t.Id
group by t.Name
having count(a.Id) in (2, 3)
order by t.Name