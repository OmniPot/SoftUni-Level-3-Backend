select
	*
from Ads
where TownId is null or CategoryId is null or ImageDataURL is null
order by Id