--Find all peaks along with their mountain sorted by elevation (from the highest to the lowest), then by peak name alphabetically.
--Display the peak name, mountain range name and elevation. Submit for evaluation the result grid with headers.

select p.PeakName, m.MountainRange as [Mountain], p.Elevation
from Peaks p
join Mountains m
	on m.Id = p.MountainId
order by p.Elevation desc, p.PeakName