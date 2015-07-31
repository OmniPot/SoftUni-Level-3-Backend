namespace NewsApp
{
    using System;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using News.Data;

    public class NewsMain
    {
        public static void Main()
        {
            var context = new NewsEntities();
            var context2 = new NewsEntities();

            // Step 1
            PrintCurrentNewsContent(context);

            try
            {
                // Step 2
                ChangeFirstNewsContentValue(context, "First");
                ChangeFirstNewsContentValue(context2, "Second");

                context.SaveChanges();
                Console.WriteLine("\n--First changes successfully saved in the DB.");
                
                context2.SaveChanges();
                Console.WriteLine("\n--Second changes successfully saved in the DB.");

                Console.WriteLine("Bye!");
            }
            catch (DbUpdateConcurrencyException cex)
            {
                Console.WriteLine(cex.Message + "\n");

                PrintCurrentNewsContent(context);
                ChangeFirstNewsContentValue(context, "Second");
            }
        }

        private static void ChangeFirstNewsContentValue(NewsEntities ctx, string user)
        {
            Console.Write("\n--" + user + " new value: ");
            var newContent = Console.ReadLine();
            ctx.News.Find(1).NewsContent = newContent;
        }

        private static void PrintCurrentNewsContent(NewsEntities context)
        {
            Console.WriteLine("News...");
            Console.WriteLine(string.Join("\n", context.News.Select(n => n.NewsContent)));
        }
    }
}
