namespace BookShopSystem.Services.Controllers
{
    using BookShopSystem.Models;
    using Models.BindingModels;
    using Models.ViewModels;
    using System.Linq;
    using System.Web.Http;

    [RoutePrefix("api/authors")]
    public class AuthorsController : BaseApiController
    {
        [HttpGet]
        [Route]
        public IHttpActionResult GetAllAuthors()
        {
            var authors = this.BookShopData.Authors.All()
                .Select(AuthorViewModel.GetViewModel);
            return this.Ok(authors);
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetAuthorById(int id)
        {
            var existingAuthor = this.BookShopData.Authors.Find(id);
            if (existingAuthor == null)
            {
                return this.BadRequest("No author with such Id.");
            }

            var viewModel = AuthorViewModel.CreateViewModel(existingAuthor);

            return this.Ok(viewModel);
        }

        [HttpPost]
        [Route]
        public IHttpActionResult CreateAuthor(AuthorBindingModel authorModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var newAuthorModel = new Author
            {
                FirstName = authorModel.FirstName,
                LastName = authorModel.LastName
            };

            this.BookShopData.Authors.Add(newAuthorModel);
            this.BookShopData.SaveChanges();

            var authorViewModel = AuthorViewModel.CreateViewModel(newAuthorModel);

            return this.Ok(authorViewModel);
        }

        [HttpGet]
        [Route("{id}/books")]
        public IHttpActionResult GetBooksByAuthorId(int id)
        {
            var existingAuthor = this.BookShopData.Authors.Find(id);
            if (existingAuthor == null)
            {
                return this.BadRequest("No author with such Id.");
            }

            var authorBooks = existingAuthor.Books.Select(BookViewModel.CreateViewModel);

            return this.Ok(authorBooks);
        }
    }
}