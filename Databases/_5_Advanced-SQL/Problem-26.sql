select d.Name as DepartmentName, e.JobTitle, e.FirstName, min(e.Salary) as MinSalary
from Employees e
join Departments d
	on e.DepartmentID = d.DepartmentID
where e.Salary = (
	select min(Salary)
	from Employees
	where DepartmentID = d.DepartmentID
)
group by d.Name, e.JobTitle, e.FirstName