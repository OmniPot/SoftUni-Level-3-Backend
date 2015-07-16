select e.FirstName + ' ' + e.LastName as EmpFirstName, m.FirstName+ ' ' + m.LastName as ManFirstName
from Employees e
left outer join Employees m
	on e.EmployeeID = m.ManagerID