namespace News.Data
{
    using System.Data.Entity;
    using Migrations;
    using Models;

    public class NewsEntities : DbContext
    {
        public NewsEntities()
            : base("NewsEntities")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<NewsEntities, Configuration>());
        }

        public virtual IDbSet<News> News { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<News>()
                .Property(t => t.RowVersion)
                .IsRowVersion();

            base.OnModelCreating(modelBuilder);
        }
    }
}