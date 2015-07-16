select e.FirstName, e.LastName, e.Salary
from Employees e
where Salary = (
	select Min(Salary)
	from Employees
	where DepartmentID = e.DepartmentID
)