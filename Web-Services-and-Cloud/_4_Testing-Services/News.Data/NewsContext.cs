namespace News.Data
{
    using System.Data.Entity;
    using Migrations;
    using Models;

    public class NewsContext : DbContext
    {
        public NewsContext()
            : base("NewsContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<NewsContext, Configuration>());
        }

        public IDbSet<News> News { get; set; }
    }
}