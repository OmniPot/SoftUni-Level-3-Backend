namespace ProductsShop.Data
{
    using System.Data.Entity;
    using Migrations;
    using Models;

    public class ProductsShopContext : DbContext
    {
        public ProductsShopContext()
            : base("ProductsShopContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ProductsShopContext, Configuration>());
        }

        public virtual IDbSet<User> Users { get; set; }

        public virtual IDbSet<Category> Categories { get; set; }

        public virtual IDbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.FriendTo)
                .WithMany(u => u.FriendWith)
                .Map(mc =>
                {
                    mc.ToTable("UserFriends");
                    mc.MapLeftKey("UserId");
                    mc.MapRightKey("FriendId");
                });

            modelBuilder.Entity<User>()
                .HasMany(u => u.ProductsSold)
                .WithRequired(s => s.Seller)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(u => u.ProductsBought)
                .WithOptional(s => s.Buyer)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}