select
	cou.CountryName as [Country],
	isnull((select p.PeakName from Peaks p
			where p.Elevation = 
				(select max(p.Elevation) from Peaks p 
					join Mountains m on m.Id = p.MountainId
					join MountainsCountries mc on mc.MountainId = m.Id
					join Countries coun on mc.CountryCode = cou.CountryCode and coun.CountryName = cou.CountryName)
	), '(no highest peak)') as [Highest Peak Name],
	isnull((select max(p.Elevation) from Peaks p
				join Mountains m on m.Id = p.MountainId
				join MountainsCountries mc on mc.MountainId = m.Id
				join Countries coun on mc.CountryCode = cou.CountryCode and coun.CountryName = cou.CountryName
	), 0) as [Highest Peak Elevation],
	isnull((select m.MountainRange from Mountains m
		join Peaks p on p.MountainId = m.Id and p.Elevation = 
		(select max(p.Elevation) from Peaks p
				join Mountains m on m.Id = p.MountainId
				join MountainsCountries mc on mc.MountainId = m.Id
				join Countries coun on mc.CountryCode = cou.CountryCode and coun.CountryName = cou.CountryName
	)), '(no mountain)') as [Mountain]
from Countries cou
order by cou.CountryName