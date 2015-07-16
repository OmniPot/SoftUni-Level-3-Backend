SELECT FirstName + ' ' + LastName as [Full Name], Salary
FROM Employees 
WHERE Salary IN (25000, 14000, 12500, 23600)