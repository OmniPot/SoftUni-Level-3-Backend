namespace BookShopSystem.Services.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using BookShopSystem.Models;
    using BookShopSystem.Models.Enumerations;

    public class BookViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public int Copies { get; set; }

        public Edition EditionType { get; set; }

        public AuthorViewModel Author { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; }

        public static Expression<Func<Book, BookViewModel>> GetViewModel
        {
            get
            {
                return book => new BookViewModel
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    Price = book.Price,
                    Copies = book.Copies,
                    Author = AuthorViewModel.CreateViewModel(book.Author),
                    EditionType = book.EditionType,
                    Categories = book.Categories.AsQueryable().Select(CategoryViewModel.CreateViewModel)
                };
            }
        }

        public static BookViewModel CreateViewModel(Book book)
        {
            return new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Price = book.Price,
                Copies = book.Copies,
                Author = AuthorViewModel.CreateViewModel(book.Author),
                EditionType = book.EditionType,
                Categories = book.Categories.AsQueryable().Select(CategoryViewModel.CreateViewModel)
            };
        }
    }
}