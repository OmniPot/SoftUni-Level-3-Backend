select top 1 t.Name as TownName, count(a.AddressID) as NumberOfEmployees
from Employees e
join Addresses a
	on e.AddressID = a.AddressID
join Towns t
	on a.TownID = t.TownId
group by t.Name
order by NumberOfEmployees desc