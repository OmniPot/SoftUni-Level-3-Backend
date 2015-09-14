namespace BookShopSystem.Services.Controllers
{
    using BookShopSystem.Models;
    using Microsoft.AspNet.Identity;
    using Models.BindingModels;
    using Models.ViewModels;
    using System;
    using System.Linq;
    using System.Web.Http;

    [RoutePrefix("api/books")]
    public class BooksController : BaseApiController
    {
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetBookById(int id)
        {
            var existingBook = this.BookShopData.Books.Find(id);
            if (existingBook == null)
            {
                return this.BadRequest("No book with that Id exists.");
            }

            var bookViewModel = BookViewModel.CreateViewModel(existingBook);

            return this.Ok(bookViewModel);
        }

        [HttpGet]
        public IHttpActionResult SearchForBookByTitle(string searchTerm)
        {
            var existingBook = this.BookShopData.Books.All()
                .Where(b => b.Title.Contains(searchTerm))
                .OrderBy(b => b.Title)
                .Take(10)
                .Select(b => new
                {
                    b.Id,
                    b.Title
                });

            return this.Ok(existingBook);
        }

        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult UpdateBookById(int id, BookBindingModel bookModel)
        {
            var existingBook = this.BookShopData.Books.Find(id);
            if (existingBook == null)
            {
                return this.BadRequest("No book with such Id.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            existingBook.Title = bookModel.Title;
            existingBook.Description = bookModel.Description;
            existingBook.Price = bookModel.Price;
            existingBook.Copies = bookModel.Copies;
            existingBook.EditionType = bookModel.EditionType;
            existingBook.AuthorId = bookModel.AuthorId;

            this.BookShopData.Books.Update(existingBook);
            this.BookShopData.SaveChanges();

            var bookViewModel = BookViewModel.CreateViewModel(existingBook);

            return this.Ok(bookViewModel);
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteBookById(int id)
        {
            var existingBook = this.BookShopData.Books.Find(id);
            if (existingBook == null)
            {
                return this.BadRequest("No book with such Id.");
            }

            this.BookShopData.Books.Delete(existingBook);
            this.BookShopData.SaveChanges();

            return this.Ok("Successfully deleted book.");
        }

        [HttpPost]
        [Route]
        public IHttpActionResult CreateBook(BookBindingModel bookModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var newBook = new Book
            {
                Title = bookModel.Title,
                Description = bookModel.Description,
                Price = bookModel.Price,
                Copies = bookModel.Copies,
                EditionType = bookModel.EditionType,
                AuthorId = bookModel.AuthorId
            };

            var newBookCategoriesNames = bookModel.Categories.Split(' ');
            this.AddCategoriesToBook(newBookCategoriesNames, newBook);

            this.BookShopData.Books.Add(newBook);
            this.BookShopData.SaveChanges();

            var bookViewModel = BookViewModel.CreateViewModel(newBook);

            return this.Ok(bookViewModel);
        }

        [Authorize]
        [HttpPut]
        [Route("buy/{id}")]
        public IHttpActionResult BuyBook(int id)
        {
            var existingBook = this.BookShopData.Books.Find(id);
            if (existingBook == null)
            {
                return this.BadRequest("No book with the specified id exists.");
            }

            if (existingBook.Copies == 0)
            {
                return this.BadRequest("Insufficient book copies.");
            }

            var newPurchase = new Purchase
            {
                BookId = existingBook.Id,
                UserId = this.User.Identity.GetUserId(),
                Price = existingBook.Price,
                DateOfPurchase = DateTime.Now,
                isRecalled = false
            };

            existingBook.Copies--;
            existingBook.Purchases.Add(newPurchase);

            this.BookShopData.Books.Update(existingBook);
            this.BookShopData.SaveChanges();

            var purchaseViewModel = PurchaseViewModel.CreateViewModel(newPurchase);

            return this.Ok(purchaseViewModel);
        }

        [Authorize]
        [HttpPut]
        [Route("recall/{id}")]
        public IHttpActionResult RecallBook(int id)
        {
            var existingBook = this.BookShopData.Books.Find(id);
            if (existingBook == null)
            {
                return this.BadRequest("No book with the specified id exists.");
            }

            var existingPurchase = existingBook.Purchases.FirstOrDefault(p =>
                    p.BookId.Equals(existingBook.Id) &&
                    p.UserId.Equals(this.User.Identity.GetUserId())
            );

            if (existingPurchase == null)
            {
                return this.BadRequest("You havent bought that book.");
            }

            if (existingPurchase.DateOfPurchase.AddDays(30) < DateTime.Now)
            {
                return this.BadRequest("You cannot recall a book if more than 30 days passed from its buy date.");
            }

            existingBook.Copies++;
            existingPurchase.isRecalled = true;

            this.BookShopData.Books.Update(existingBook);
            this.BookShopData.SaveChanges();

            var purchaseViewModel = PurchaseViewModel.CreateViewModel(existingPurchase);

            return this.Ok(purchaseViewModel);
        }

        private void AddCategoriesToBook(string[] categoryNames, Book book)
        {
            foreach (var categoryName in categoryNames)
            {
                var existingCategory = this.BookShopData.Categories.All()
                    .FirstOrDefault(c => c.Name.Equals(categoryName));
                if (existingCategory != null)
                {
                    book.Categories.Add(existingCategory);
                }
            }
        }
    }
}