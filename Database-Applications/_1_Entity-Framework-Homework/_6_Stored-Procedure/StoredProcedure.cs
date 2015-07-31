using System;
using _6_Stored_Procedure;

public class StoredProcedure
{
    public static void Main()
    {
        var context = new SoftUniEntities();

        var projectsByEmployee = context.usp_pojects_by_employee_name("Ruth", "Ellerbrock");

        foreach (var project in projectsByEmployee)
        {
            Console.WriteLine(project);
        }
    }
}