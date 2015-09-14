namespace BookShopSystem.Services.Models.ViewModels
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using BookShopSystem.Models;

    public class PurchaseViewModel
    {
        public int Id { get; set; }

        public string BookTitle { get; set; }

        public double Price { get; set; }

        public string SoldAt { get; set; }

        public bool IsRecalled { get; set; }

        public static Expression<Func<Purchase, PurchaseViewModel>> GetViewModel
        {
            get
            {
                return purchase => new PurchaseViewModel
                {
                    Id = purchase.Id,
                    BookTitle = purchase.Book.Title,
                    Price = purchase.Price,
                    SoldAt = purchase.DateOfPurchase.ToShortDateString(),
                    IsRecalled = purchase.isRecalled
                };
            }
        }

        public static PurchaseViewModel CreateViewModel(Purchase purchase)
        {
            return new PurchaseViewModel
            {
                Id = purchase.Id,
                BookTitle = purchase.Book.Title,
                Price = purchase.Price,
                SoldAt = purchase.DateOfPurchase.ToShortDateString(),
                IsRecalled = purchase.isRecalled
            };
        }
    }
}
