select e.FirstName + ' ' + e.LastName as EmpFirstName, m.FirstName+ ' ' + m.LastName as ManFirstName
from Employees e
right outer join Employees m
	on e.EmployeeID = m.ManagerID