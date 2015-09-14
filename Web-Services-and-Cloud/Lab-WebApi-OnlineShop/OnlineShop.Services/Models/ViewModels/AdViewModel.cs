namespace OnlineShop.Services.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using OnlineShop.Models;

    public class AdViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public DateTime PostedOn { get; set; }

        public DateTime? ClosedOn { get; set; }

        public AdStatus Status { get; set; }

        public UserViewModel Owner { get; set; }

        public AdTypeViewModel Type { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; }

        public static Expression<Func<Ad, AdViewModel>> ToViewModel
        {
            get
            {
                return ad => new AdViewModel
                {
                    Id = ad.Id,
                    Name = ad.Name,
                    Description = ad.Description,
                    Price = ad.Price,
                    Owner = new UserViewModel
                    {
                        Id = ad.OwnerId,
                        Username = ad.Owner.UserName
                    },
                    Type = new AdTypeViewModel
                    {
                        Id = ad.TypeId,
                        Name = ad.Type.Name,
                        PricePerDay = ad.Type.PricePerDay
                    },
                    PostedOn = ad.PostedOn,
                    Categories = ad.Categories.AsQueryable().Select(CategoryViewModel.GetViewModel)
                };
            }
        }
    }
}