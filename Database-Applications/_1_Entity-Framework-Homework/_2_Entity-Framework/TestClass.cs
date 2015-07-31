using System;
using Entity_Framework_Homework;

public class TestClass
{
    public static void Main()
    {
        // Creating and adding an Employee to the database.
        var newEmployee = new Employee
        {
            FirstName = "Gruio",
            LastName = "Gruev",
            HireDate = DateTime.Now,
            JobTitle = "Software Engineer",
            DepartmentID = 2
        };

        DataAccessObject.Add(newEmployee);

        // Finding an employee using its primary key. You need to select an existing key!
        var empFound = DataAccessObject.FindByKey(200);

        // Modifying and employee
        DataAccessObject.Modify(empFound);

        // Removing an employee
        var empToRemove = DataAccessObject.FindByKey(279);
        DataAccessObject.Delete(empToRemove);
    }
}