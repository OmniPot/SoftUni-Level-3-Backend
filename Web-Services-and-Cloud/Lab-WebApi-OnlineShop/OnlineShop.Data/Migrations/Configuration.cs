namespace OnlineShop.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<OnlineShopContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(OnlineShopContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            var adTypes = SeedAdTypes(context);

            var user = this.SeedUser(context);

            this.SeedCategories(context);

            SeedAds(context, user, adTypes);
        }

        private static void SeedAds(OnlineShopContext context, ApplicationUser user, List<AdType> adTypes)
        {
            user.OwnAds.Add(new Ad()
            {
                Name = "Motika",
                Description = "Brand new",
                Price = 199.99m,
                PostedOn = DateTime.Now.AddDays(-6),
                Type = adTypes[0]
            });

            user.OwnAds.Add(new Ad()
            {
                Name = "Tarnokop",
                Description = "Old antique",
                Price = 15.20m,
                PostedOn = DateTime.Now.AddDays(-12),
                Type = adTypes[1]
            });

            context.SaveChanges();
        }

        private static List<AdType> SeedAdTypes(OnlineShopContext context)
        {
            var adTypes = new List<AdType>
            {
                new AdType()
                {
                    Name = "Normal",
                    PricePerDay = 3.99m,
                    Index = 100
                },
                new AdType()
                {
                    Name = "Premium",
                    PricePerDay = 5.99m,
                    Index = 200
                },
                new AdType()
                {
                    Name = "Diamond",
                    PricePerDay = 9.99m,
                    Index = 300
                }
            };

            foreach (var adType in adTypes)
            {
                context.AdTypes.Add(adType);
            }

            context.SaveChanges();

            return adTypes;
        }

        private ApplicationUser SeedUser(OnlineShopContext context)
        {
            var user = new ApplicationUser()
            {
                UserName = "bigSmoke",
                Email = "richard@gmail.bg"
            };

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            const string password = "bigSm0ke!";

            var userCreateResult = userManager.Create(user, password);
            if (!userCreateResult.Succeeded)
            {
                throw new InvalidOperationException(string.Join(
                    Environment.NewLine,
                    userCreateResult.Errors));
            }

            return user;
        }

        private void SeedCategories(OnlineShopContext context)
        {
            var categories = new List<Category>()
            {
                new Category() {Name = "Business"},
                new Category() {Name = "Garden"},
                new Category() {Name = "Toys"},
                new Category() {Name = "Pleasure"},
                new Category() {Name = "Electronics"},
                new Category() {Name = "Clothes"}
            };

            foreach (var category in categories)
            {
                context.Categories.Add(category);
            }

            context.SaveChanges();
        }
    }
}