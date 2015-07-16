select avg(Salary) as [Average Salary for Department #1]
from Employees e
join Departments d
	on d.DepartmentID = e.DepartmentID
where d.Name = 'Sales'