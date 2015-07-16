SELECT d.Name as Department, e.JobTitle, AVG(Salary) as AVGSalery
FROM Employees e
JOIN Departments d
	ON e.DepartmentID = d.DepartmentID
group by d.Name, e.JobTitle