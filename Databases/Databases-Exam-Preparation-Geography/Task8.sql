--Find the highest, lowest and average peak elevation. Submit for evaluation the result grid with headers.

select
	MAX(Elevation) as [MaxElevation],
	MIN(Elevation) as [MinElevation],
	AVG(Elevation) as [AverageElevation]
from Peaks