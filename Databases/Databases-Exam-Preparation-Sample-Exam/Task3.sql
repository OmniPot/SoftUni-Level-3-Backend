select
	Title,
	Date,
	(case when ImageDataURL is null then 'no' else 'yes' end) as [Has Image]
from Ads
order by Id