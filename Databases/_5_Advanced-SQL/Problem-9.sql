select d.Name, avg(e.Salary)
from Departments d
join Employees e
	on d.DepartmentID = e.DepartmentID
group by d.Name