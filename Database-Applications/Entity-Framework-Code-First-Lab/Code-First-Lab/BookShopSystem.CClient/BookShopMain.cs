namespace BookShopSystem.CClient
{
    using System;
    using System.Linq;
    using Data;

    public class BookShopMain
    {
        public static void Main()
        {
            var context = new BookShopSystemEntities();

            // 1
            GetBooksWithReleaseYearAfter2000(context);

            // 2
            GetAuthorsWithABookAfter1990(context);

            // 3
            GetAuthorsSortedByNumberOfBooks(context);

            // 4
            GetBooksOfGeorgePowell(context);

            // 5
            GetCategoriesSortedByCountOfBooks(context);

            TestRelatedBooksExtendedFunctionality(context);
        }

        private static void TestRelatedBooksExtendedFunctionality(BookShopSystemEntities context)
        {
            var books = context.Books.Take(3).ToList();
            books[0].RelatedBooks.Add(books[1]);
            books[1].RelatedBooks.Add(books[0]);
            books[0].RelatedBooks.Add(books[2]);
            books[2].RelatedBooks.Add(books[0]);

            context.SaveChanges();

            var booksFromQuery = context.Books
                .Take(3)
                .Select(b => new
                {
                    b.Title,
                    b.RelatedBooks
                });

            foreach (var book in booksFromQuery)
            {
                Console.WriteLine("--{0}", book.Title);
                foreach (var relatedBook in book.RelatedBooks)
                {
                    Console.WriteLine(relatedBook.Title);
                }
            }
        }

        private static void GetCategoriesSortedByCountOfBooks(BookShopSystemEntities context)
        {
            var latest3BooksFromEachCategory = context.Categories
                .OrderByDescending(c => c.Books.Count)
                .Select(c => new
                {
                    Category = c.Name,
                    BooksCount = c.Books.Count,
                    Books = c.Books
                        .OrderByDescending(b => b.ReleaseDate)
                        .Take(3)
                        .Select(b => new
                        {
                            b.Title,
                            b.ReleaseDate
                        })
                });
        }

        private static void GetBooksOfGeorgePowell(BookShopSystemEntities context)
        {
            var georgePowellsBooks = context.Books
                .Where(b => b.Author.FirstName == "George" && b.Author.LastName == "Powell")
                .OrderBy(b => b.ReleaseDate)
                .ThenBy(b => b.Title)
                .Select(b => new
                {
                    b.Title,
                    b.ReleaseDate,
                    b.Copies
                });
        }

        private static void GetAuthorsSortedByNumberOfBooks(BookShopSystemEntities context)
        {
            var authorsSortedByBooksCount = context.Authors
                .OrderByDescending(a => a.Books.Count)
                .Select(a => new
                {
                    a.FirstName,
                    a.LastName,
                    BooksCount = a.Books.Count
                });
        }

        private static void GetAuthorsWithABookAfter1990(BookShopSystemEntities context)
        {
            var authorsWithABookAfter1990 = context.Authors
                .Where(a => a.Books.Count(b => b.ReleaseDate.Value.Year < 1990) >= 1)
                .Select(a => new
                {
                    a.FirstName,
                    a.LastName,
                    BookCount = a.Books.Count(b => b.ReleaseDate.Value.Year < 1990)
                });
        }

        private static void GetBooksWithReleaseYearAfter2000(BookShopSystemEntities context)
        {
            var booksWithReleaseYearAfter2000 = context.Books
                .Where(b => b.ReleaseDate.Value.Year > 2000)
                .Select(b => b.Title);
        }
    }
}