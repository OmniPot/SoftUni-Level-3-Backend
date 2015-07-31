namespace ATM.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ATMEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ATMEntities context)
        {
            if (!context.CardAccounts.Any())
            {
                var random = new Random();

                for (int i = 0; i < 10; i++)
                {
                    var number = random.Next(1000000000).ToString("D10");
                    var pin = random.Next(9999).ToString("D4");
                    var account = new CardAccount
                    {
                        CardNumber = number,
                        CardCash = random.Next(200, 50000),
                        CardPIN = pin
                    };

                    context.CardAccounts.Add(account);
                }
            }
        }
    }
}
