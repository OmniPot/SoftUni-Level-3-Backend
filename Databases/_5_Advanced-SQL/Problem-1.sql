select FirstName + ' ' + LastName, Salary
from Employees
where Salary = (
	select Min(Salary) 
	from Employees
)