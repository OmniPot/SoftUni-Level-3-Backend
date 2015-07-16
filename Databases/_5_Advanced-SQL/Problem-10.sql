select t.Name as [Town Name], d.Name as [Department Name], count(*) as [Employees in Department]
from Employees e
join Departments d
	on e.DepartmentID = d.DepartmentID
join Addresses a
	on e.AddressID = a.AddressID
join Towns t
	on a.TownID = t.TownID
group by d.Name, t.Name