using System.Linq;
using _5_Concurent_Database_Changes_EF;

public class ConcurentChanges
{
    public static void Main()
    {
        var firstContext = new SoftUniEntities();
        var secondContext = new SoftUniEntities();

        var employee1 = firstContext.Employees.First();
        var employee2 = secondContext.Employees.First();

        employee1.FirstName = "Stoqn";
        employee2.FirstName = "Petar";

        firstContext.SaveChanges();
        secondContext.SaveChanges();

        // 1. Optimistic concurency (default) means that the name of the employee will be Petar
        // 2. Pesimistic concurency (Fixed) mean that the name of the employee will be Stoqn
    }
}
