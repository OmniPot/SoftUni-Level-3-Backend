SELECT e.FirstName + ' ' + e.LastName, m.FirstName + ' ' + m.LastName
FROM Employees e, Employees m
WHERE e.ManagerID = m.EmployeeID