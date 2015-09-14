namespace BookShopSystem.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Web.Hosting;
    using Models;
    using Models.Enumerations;

    public sealed class Configuration : DbMigrationsConfiguration<BookShopSystemDbContext>
    {
        private readonly string DataSourceLocation = HostingEnvironment.MapPath("~/App_Data/");

        private const string BooksDataFileName = "books.txt";
        private const string AuthorsDataFileName = "authors.txt";
        private const string CategoriesDataFileName = "categories.txt";

        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(BookShopSystemDbContext dbContext)
        {
            this.SeedAuthors(dbContext);
            this.SeedCategories(dbContext);
            this.SeedBooks(dbContext);
        }

        private void SeedBooks(BookShopSystemDbContext dbContext)
        {
            var random = new Random();
            var authors = dbContext.Authors.ToList();

            if (!dbContext.Books.Any())
            {
                using (var reader = new StreamReader(this.DataSourceLocation + BooksDataFileName))
                {
                    var line = reader.ReadLine();
                    line = reader.ReadLine();
                    while (line != null)
                    {
                        var data = line.Split(new[] { ' ' }, 6);

                        var title = data[5];
                        var price = double.Parse(data[3]);
                        var copies = int.Parse(data[2]);
                        var authorIndex = random.Next(0, authors.Count);
                        var author = authors[authorIndex];
                        var edition = (Edition)int.Parse(data[0]);

                        var categoryNumber = random.Next(1, 3);
                        var categories = new HashSet<Category>();
                        for (var i = 0; i < categoryNumber; i++)
                        {
                            var categoryId = random.Next(1, 8);
                            var newCategory = dbContext.Categories.First(c => c.Id == categoryId);
                            categories.Add(newCategory);
                        }

                        dbContext.Books.Add(new Book
                        {
                            Title = title,
                            Price = price,
                            Copies = copies,
                            EditionType = edition,
                            Author = author,
                            Categories = categories
                        });

                        line = reader.ReadLine();
                    }
                }
            }

            dbContext.SaveChanges();
        }

        private void SeedCategories(BookShopSystemDbContext dbContext)
        {
            if (!dbContext.Categories.Any())
            {
                using (var reader = new StreamReader(this.DataSourceLocation + CategoriesDataFileName))
                {
                    var line = reader.ReadLine();
                    line = reader.ReadLine();
                    while (line != null)
                    {
                        var data = line.Split(' ');
                        dbContext.Categories.Add(new Category
                        {
                            Name = data[0]
                        });

                        line = reader.ReadLine();
                    }
                }
            }

            dbContext.SaveChanges();
        }

        private void SeedAuthors(BookShopSystemDbContext dbContext)
        {
            if (!dbContext.Authors.Any())
            {
                using (var reader = new StreamReader(this.DataSourceLocation + AuthorsDataFileName))
                {
                    var line = reader.ReadLine();
                    line = reader.ReadLine();
                    while (line != null)
                    {
                        var data = line.Split(' ');
                        dbContext.Authors.Add(new Author
                        {
                            FirstName = data[0],
                            LastName = data[1]
                        });

                        line = reader.ReadLine();
                    }
                }
            }

            dbContext.SaveChanges();
        }
    }
}