select e.FirstName + ' ' + e.LastName as [Employee Name], d.Name as [Department Name], e.HireDate
from Employees e
join Departments d
	on e.DepartmentID = d.DepartmentID
where e.HireDate 
	between '12-12-1995' and '1-1-2005' 
	and d.Name in ('Sales', 'Finance')