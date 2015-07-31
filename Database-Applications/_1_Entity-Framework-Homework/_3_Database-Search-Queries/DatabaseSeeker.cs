using System;
using System.Linq;
using _3_Database_Search_Queries;

public class DatabaseSeeker
{
    public static void Main()
    {
        var entities = new SoftUniEntities();

        // Query 1
        GetFirstQueryResults(entities);

        // Query 2
        GetSecondQueryResults(entities);

        // Query 3
        GetThirdQueryResults(entities);

        // Query 4
        GetFourthQueryResults(entities);
    }

    private static void GetFourthQueryResults(SoftUniEntities entities)
    {
        var departments = entities.Departments
            .Where(dep => dep.Employees.Count > 5)
            .OrderBy(dep => dep.Employees.Count)
            .Select(dep => new
            {
                Name = dep.Name,
                ManagerName = dep.Employees
                    .Where(emp => emp.EmployeeID == dep.ManagerID)
                    .Select(emp => emp.FirstName + " " + emp.LastName).FirstOrDefault(),
                Employees = dep.Employees.Select(emp => new
                {
                    emp.FirstName,
                    emp.LastName,
                    emp.JobTitle,
                    emp.HireDate
                })
            });

        Console.WriteLine();

        foreach (var department in departments)
        {
            Console.WriteLine("--{0} - Manager: {1}, Employees: {2}",
                department.Name,
                department.ManagerName,
                department.Employees.Count());
        }
    }

    private static void GetThirdQueryResults(SoftUniEntities entities)
    {
        var foundUser = entities.Employees
            .Where(em => em.EmployeeID == 147)
            .Select(em => new
            {
                em.FirstName,
                em.LastName,
                em.JobTitle,
                Projects = em.Projects
                    .OrderBy(proj => proj.Name)
                    .Select(proj => proj.Name)
            });

        Console.WriteLine();
        foreach (var em in foundUser)
        {
            Console.WriteLine("First name: " + em.FirstName);
            Console.WriteLine("Last name: " + em.LastName);
            Console.WriteLine("Job title: " + em.JobTitle);

            Console.WriteLine(string.Join(", ", em.Projects));
        }
    }

    private static void GetSecondQueryResults(SoftUniEntities entities)
    {
        var adressesFound = entities.Addresses
            .OrderByDescending(adr => adr.Employees.Count)
            .ThenBy(adr => adr.Town.Name)
            .Select(adr => new
            {
                AddressText = adr.AddressText,
                TownName = adr.Town.Name,
                EmployeeCount = adr.Employees.Count
            })
            .Take(10);

        foreach (var adr in adressesFound)
        {
            Console.WriteLine("Adress text: " + adr.AddressText);
            Console.WriteLine("Town name: " + adr.TownName);
            Console.WriteLine("Employee count: " + adr.EmployeeCount);
        }
    }

    private static void GetFirstQueryResults(SoftUniEntities sue)
    {
        var empWithProjectInPeriod = sue.Employees
            .Where(e => e.Projects
                .Count(pro => pro.StartDate >= new DateTime(2001, 1, 1) &&
                       pro.StartDate < new DateTime(2003, 12, 31)) >= 1)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    ManagerName = e.Employee1.FirstName + e.Employee1.LastName,
                    Projects = e.Projects.Select(p => new
                    {
                        p.Name,
                        p.StartDate,
                        p.EndDate
                    })
                });

        foreach (var employee in empWithProjectInPeriod)
        {
            Console.WriteLine("Employeee Name: " + employee.FirstName + " " + employee.LastName);
            Console.WriteLine("Manager Name: " + employee.ManagerName);
            Console.WriteLine("Projects: ");
            foreach (var project in employee.Projects)
            {
                Console.WriteLine("--" + project.Name);
                Console.WriteLine("----" + project.StartDate);
                Console.WriteLine("----" + project.EndDate);
            }

            Console.WriteLine();
        }
    }
}

