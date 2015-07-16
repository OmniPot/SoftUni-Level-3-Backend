select count(*) as [Sales Employees Count]
from Employees e
join Departments d
	on d.DepartmentID = e.DepartmentID
where d.Name = 'Sales'