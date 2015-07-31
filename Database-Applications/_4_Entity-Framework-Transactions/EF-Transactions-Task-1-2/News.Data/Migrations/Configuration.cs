namespace News.Data.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<NewsEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(NewsEntities context)
        {
            if (!context.News.Any())
            {
                var news = new string[]
                {
                    "I am the king!",
                    "The New York major left the building...",
                    "Where ur parents are atm? No idea? Batman knows where his are!"
                };

                foreach (var n in news)
                {
                    context.News.Add(new News
                    {
                        NewsContent = n
                    });
                }
            }
        }
    }
}
