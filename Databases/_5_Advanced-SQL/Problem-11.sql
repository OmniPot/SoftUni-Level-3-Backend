select m.FirstName, m.LastName, count(m.FirstName) as [ManagedEmployeesCount]
from Employees e
join Employees m
	on e.ManagerID = m.EmployeeID
group by m.FirstName, m.LastName
having count(*) = 5