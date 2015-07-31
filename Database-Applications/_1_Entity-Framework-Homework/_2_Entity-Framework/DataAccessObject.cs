using Entity_Framework_Homework;

public class DataAccessObject
{
    private static readonly SoftUniEntities SoftUniEntities = new SoftUniEntities();

    public static void Add(Employee employee)
    {
        SoftUniEntities.Employees.Add(employee);

        SoftUniEntities.SaveChanges();
    }

    public static Employee FindByKey(object key)
    {
        var empFound = SoftUniEntities.Employees.Find(key);

        return empFound;
    }

    public static void Modify(Employee employee)
    {
        employee.FirstName = "Mitko";

        SoftUniEntities.SaveChanges();
    }

    public static void Delete(Employee employee)
    {
        SoftUniEntities.Employees.Remove(employee);

        SoftUniEntities.SaveChanges();
    }
}