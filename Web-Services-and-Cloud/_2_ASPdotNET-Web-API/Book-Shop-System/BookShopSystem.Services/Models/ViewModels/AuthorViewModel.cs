namespace BookShopSystem.Services.Models.ViewModels
{
    using System;
    using System.Linq.Expressions;
    using BookShopSystem.Models;

    public class AuthorViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public static Expression<Func<Author, AuthorViewModel>> GetViewModel
        {
            get
            {
                return author => new AuthorViewModel
                {
                    Id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName
                };
            }
        }

        public static AuthorViewModel CreateViewModel(Author a)
        {
            return new AuthorViewModel
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName
            };
        }
    }
}