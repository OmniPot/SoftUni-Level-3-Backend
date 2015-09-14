namespace Messages.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public sealed class MessagesDbMigrationConfiguration : DbMigrationsConfiguration<MessagesDbContext>
    {
        public MessagesDbMigrationConfiguration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(MessagesDbContext context)
        {
        }
    }
}