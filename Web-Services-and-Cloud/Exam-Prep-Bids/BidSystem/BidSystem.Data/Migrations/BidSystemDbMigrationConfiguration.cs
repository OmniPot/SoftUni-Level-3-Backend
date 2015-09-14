namespace BidSystem.Data.Migrations
{
    using System.Data.Entity.Migrations;

    using BidSystem.Data;

    public sealed class BidSystemDbMigrationConfiguration :
        DbMigrationsConfiguration<BidSystemDbContext>
    {
        public BidSystemDbMigrationConfiguration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(BidSystemDbContext context)
        {
        }
    }
}
