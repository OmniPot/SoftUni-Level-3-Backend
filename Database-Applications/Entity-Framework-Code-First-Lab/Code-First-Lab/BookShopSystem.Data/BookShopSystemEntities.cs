namespace BookShopSystem.Data
{
    using System.Data.Entity;
    using Migrations;
    using Models;

    public class BookShopSystemEntities : DbContext
    {
        public BookShopSystemEntities()
            : base("BookShopSystemEntities")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BookShopSystemEntities, Configuration>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasMany(b => b.RelatedBooks)
                .WithMany()
                .Map(m =>
                {
                    m.MapLeftKey("Book_Id");
                    m.MapRightKey("RelatedBook_Id");
                    m.ToTable("RelatedBooks");
                });

            base.OnModelCreating(modelBuilder);
        }

        public virtual IDbSet<Book> Books { get; set; }

        public virtual IDbSet<Author> Authors { get; set; }

        public virtual IDbSet<Category> Categories { get; set; }
    }
}