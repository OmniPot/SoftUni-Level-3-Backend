select t.Name as [Town], count(t.TownID) as [Number of managers]
from Employees e
join Addresses a 
	on e.AddressID = a.AddressID
join Towns t 
	on a.TownID = t.TownID
where e.EmployeeID IN (
	select ManagerID from Employees
)
group by t.TownID, t.Name