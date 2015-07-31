namespace ProductsShop.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using Models;
    using Newtonsoft.Json;

    internal sealed class Configuration : DbMigrationsConfiguration<ProductsShopContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ProductsShopContext context)
        {
            SeedUsersData(context);
            SeedUserFriendsData(context);
            SeedCategoriesData(context);
            SeedProductsData(context);
        }

        private void SeedUserFriendsData(ProductsShopContext context)
        {
            if (context.Users.Find(1).FriendTo.Count == 0)
            {
                var random = new Random();

                foreach (var user in context.Users)
                {
                    var friendsCount = random.Next(1, 10);
                    var usersFriendTo = new HashSet<User>();
                    var usersFriendWith = new HashSet<User>();

                    for (int i = 0; i < friendsCount; i++)
                    {
                        var friendToId = random.Next(1, context.Users.Count());
                        var friendWithId = random.Next(1, context.Users.Count());

                        usersFriendTo.Add(context.Users.Find(friendToId));
                        usersFriendWith.Add(context.Users.Find(friendWithId));
                    }

                    user.FriendTo = usersFriendTo;
                    user.FriendWith = usersFriendWith;
                }

                context.SaveChanges();
            }
        }

        private void SeedCategoriesData(ProductsShopContext context)
        {
            if (!context.Categories.Any())
            {
                string jsonText = File.ReadAllText("../../categories.json");
                var categoriesData = JsonConvert.DeserializeObject<List<Category>>(jsonText);

                foreach (var category in categoriesData)
                {
                    context.Categories.Add(category);
                }

                context.SaveChanges();
            }           
        }

        private void SeedProductsData(ProductsShopContext context)
        {
            if (!context.Products.Any())
            {
                string jsonText = File.ReadAllText("../../products.json");
                var productsData = JsonConvert.DeserializeObject<List<Product>>(jsonText);
                var random = new Random();

                foreach (var product in productsData)
                {
                    var categoriesToAdd = new HashSet<Category>();
                    var categoriesCount = random.Next(3);

                    var sellerId = random.Next(1, context.Users.Count() + 1);
                    var seller = context.Users.Find(sellerId);

                    var buyerId = random.Next(1, context.Users.Count() + 1);
                    var buyer = context.Users.Find(buyerId);

                    for (int i = 0; i < categoriesCount; i++)
                    {
                        var categoryId = random.Next(context.Categories.Count() + 1);
                        categoriesToAdd.Add(context.Categories.Find(categoryId));
                    }

                    var productToAdd = new Product
                    {
                        Name = product.Name,
                        Price = product.Price,
                        Categories = categoriesToAdd,
                        Seller = seller,
                        Buyer = buyer,
                    };

                    buyer.ProductsBought.Add(productToAdd);
                    seller.ProductsSold.Add(productToAdd);

                    context.Products.Add(productToAdd);
                }

                context.SaveChanges();
            }
        }

        private void SeedUsersData(ProductsShopContext context)
        {
            if (!context.Users.Any())
            {
                var usersDocument = XDocument.Load("../../users.xml");
                var userDocData =
                    from user in usersDocument.Root.Descendants("user")
                    select new User
                    {
                        FirstName = user.Attribute("first-name") != null ? user.Attribute("first-name").Value : null,
                        LastName = user.Attribute("last-name") != null ? user.Attribute("last-name").Value : null,
                        Age = user.Attribute("age") != null ? int.Parse(user.Attribute("age").Value) : (int?)null
                    };

                foreach (var user in userDocData)
                {
                    context.Users.Add(user);
                }

                context.SaveChanges();
            }
        }
    }
}
