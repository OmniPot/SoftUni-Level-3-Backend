namespace BookShopSystem.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Migrations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using Enumerations;
    using Models;
    using Models.Enumerations;

    public sealed class Configuration : DbMigrationsConfiguration<BookShopSystemEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "BookShopSystem.Data.BookShopSystemEntities";
        }

        protected override void Seed(BookShopSystemEntities context)
        {
            if (!context.Authors.Any())
            {
                using (var reader = new StreamReader("authors.txt"))
                {
                    var line = reader.ReadLine();
                    line = reader.ReadLine();
                    while (line != null)
                    {
                        var data = line.Split(' ');
                        context.Authors.Add(new Author()
                        {
                            FirstName = data[0],
                            LastName = data[1]
                        });

                        line = reader.ReadLine();
                    }
                }
            }

            context.SaveChanges();

            if (!context.Categories.Any())
            {
                using (var reader = new StreamReader("categories.txt"))
                {
                    var line = reader.ReadLine();
                    line = reader.ReadLine();
                    while (line != null)
                    {
                        var data = line.Split(' ');
                        context.Categories.Add(new Category()
                        {
                            Name = data[0]
                        });

                        line = reader.ReadLine();
                    }
                }
            }

            context.SaveChanges();

            var random = new Random();
            var authors = context.Authors.ToList();

            if (!context.Books.Any())
            {
                using (var reader = new StreamReader("books.txt"))
                {
                    var line = reader.ReadLine();
                    line = reader.ReadLine();
                    while (line != null)
                    {
                        var data = line.Split(new[] { ' ' }, 6);
                        var authorIndex = random.Next(0, authors.Count);
                        var author = authors[authorIndex];
                        var edition = (Edition)int.Parse(data[0]);
                        var releaseDate = DateTime.ParseExact(data[1], "d/M/yyyy", CultureInfo.InvariantCulture);
                        var copies = int.Parse(data[2]);
                        var price = double.Parse(data[3]);
                        var ageRestriction = (AgeRestriction)int.Parse(data[4]);
                        var title = data[5];

                        var categoryNumber = random.Next(1, 3);
                        var categories = new HashSet<Category>();
                        for (int i = 0; i < categoryNumber; i++)
                        {
                            var categoryId = random.Next(1, 8);
                            var newCategory = context.Categories.First(c => c.Id == categoryId);
                            categories.Add(newCategory);
                        }

                        context.Books.Add(new Book()
                        {
                            Author = author,
                            EditionType = edition,
                            ReleaseDate = releaseDate,
                            Copies = copies,
                            Price = price,
                            AgeRestriction = ageRestriction,
                            Title = title,
                            Categories = categories
                        });

                        line = reader.ReadLine();
                    }
                }
            }

            context.SaveChanges();
        }
    }
}
