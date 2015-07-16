--Combine all peak names with all river names, so that the last letter of each peak name
--is the same like the first letter of its corresponding river name. Display the peak names,
--river names, and the obtained mix. Sort the results by the obtained mix.
--Submit for evaluation the result grid with headers.

select 
	p.PeakName, 
	r.RiverName,
	lower(substring(p.peakName, 1, LEN(p.PeakName) - 1) + r.RiverName) as Mix
from Peaks p, Rivers r
where right(p.PeakName, 1) = LEFT(r.RiverName, 1)
order by Mix