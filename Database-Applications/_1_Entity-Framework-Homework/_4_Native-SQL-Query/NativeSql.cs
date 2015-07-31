using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using _4_Native_SQL_Query;

public class NativeSql
{
    public static void Main()
    {
        var context = new SoftUniEntities();

        var totalCount = context.Employees.Count();

        var stopWatch = new Stopwatch();
        stopWatch.Start();
        PrintNamesWithNativeQuery(context);
        var res = stopWatch.Elapsed;

        stopWatch.Restart();
        PrintNamesWithLinqQuery(context);
        var res2 = stopWatch.Elapsed;



        Console.WriteLine("Native : {0}", res);
        Console.WriteLine("Linq : {0}", res2);

        stopWatch.Stop();
    }

    private static void PrintNamesWithLinqQuery(SoftUniEntities context)
    {
        var foundEmps = context.Projects
            .Where(proj => proj.StartDate.Year == 2002)
            .Select(proj => proj.Employees
                .Select(emp => new { FirstName = emp.FirstName }));

        //foreach (var employee in foundEmps)
        //{
        //    foreach (var emp in employee)
        //    {
        //        Console.WriteLine(emp.FirstName);
        //    }
        //}
    }

    private static void PrintNamesWithNativeQuery(SoftUniEntities context)
    {
        var result = context.Database.SqlQuery<string>("SELECT e.FirstName " +
                                                "FROM Employees e " +
                                                "JOIN EmployeesProjects ep " +
                                                "    ON ep.EmployeeID = e.EmployeeID " +
                                                "JOIN Projects p " +
                                                "    ON p.ProjectID = ep.ProjectID " +
                                                "WHERE DATENAME(YEAR ,p.StartDate) = 2002 " +
                                                "GROUP BY e.FirstName ");

        //foreach (var employee in result)
        //{
        //    Console.WriteLine(employee);
        //}
    }
}