namespace BookShopSystem.Data
{
    using System.Data.Entity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Migrations;
    using Models;

    public class BookShopSystemDbContext : IdentityDbContext<User>, IBookShopSystemDbContext
    {
        public BookShopSystemDbContext()
            : base("BookShopSystemDbContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BookShopSystemDbContext, Configuration>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Books)
                .WithMany(b => b.Categories)
                .Map(m =>
                {
                    m.MapLeftKey("CategoryId");
                    m.MapRightKey("BookId");
                    m.ToTable("CategoryBooks");
                });

            base.OnModelCreating(modelBuilder);
        }

        public virtual IDbSet<Book> Books { get; set; }

        public virtual IDbSet<Author> Authors { get; set; }

        public virtual IDbSet<Category> Categories { get; set; }

        public IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public new void SaveChanges()
        {
            base.SaveChanges();
        }

        public static BookShopSystemDbContext Create()
        {
            return new BookShopSystemDbContext();
        }
    }
}