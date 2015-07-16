BEGIN TRAN
DELETE a
FROM Employees a
JOIN Departments d
	ON a.DepartmentID = d.DepartmentID
WHERE d.Name = 'Sales'
ROLLBACK TRAN