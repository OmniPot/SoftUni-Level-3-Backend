namespace BookShopSystem.Services.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using BookShopSystem.Models;
    using Models.BindingModels;
    using Models.ViewModels;

    [RoutePrefix("api/categories")]
    public class CategoriesController : BaseApiController
    {
        [HttpGet]
        [Route]
        public IHttpActionResult GetAll()
        {
            var categories = this.BookShopData.Categories.All()
                .Select(CategoryViewModel.GetViewModel);

            return this.Ok(categories);
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetCategoryById(int id)
        {
            var existingCategory = this.BookShopData.Categories.Find(id);
            if (existingCategory == null)
            {
                return this.BadRequest("No author with such Id.");
            }

            var categoryViewModel = CategoryViewModel.CreateViewModel(existingCategory);

            return this.Ok(categoryViewModel);
        }

        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult UpdateCategoryById(int id, CategoryBindingModel categoryModel)
        {
            var existingCategoryById = this.BookShopData.Categories.Find(id);
            var existingCategoryByName = this.BookShopData.Categories.All()
                .FirstOrDefault(a => a.Name.Equals(categoryModel.Name));

            if (existingCategoryById == null)
            {
                return this.BadRequest("No author with such Id.");
            }

            if (existingCategoryByName != null)
            {
                return this.BadRequest("Category with the specified name already exists.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            existingCategoryById.Name = categoryModel.Name;

            this.BookShopData.Categories.Update(existingCategoryById);
            this.BookShopData.SaveChanges();

            var categoryViewModel = CategoryViewModel.CreateViewModel(existingCategoryById);

            return this.Ok(categoryViewModel);
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteCategoryById(int id)
        {
            var existingCategory = this.BookShopData.Categories.Find(id);
            if (existingCategory == null)
            {
                return this.BadRequest("No category with such Id.");
            }

            this.BookShopData.Categories.Delete(existingCategory);
            this.BookShopData.SaveChanges();

            return this.Ok("Successfully deleted category.");
        }

        [HttpPost]
        [Route]
        public IHttpActionResult AddCategory(CategoryBindingModel categoryModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var existingCategory = this.BookShopData.Categories.All()
                .FirstOrDefault(c => c.Name.Equals(categoryModel.Name));

            if (existingCategory != null)
            {
                return this.BadRequest("Category with the specified name already exists.");
            }

            var newCategory = new Category
            {
                Name = categoryModel.Name
            };

            this.BookShopData.Categories.Add(newCategory);
            this.BookShopData.SaveChanges();

            var categoryViewModel = CategoryViewModel.CreateViewModel(newCategory);

            return this.Ok(categoryViewModel);
        }
    }
}
