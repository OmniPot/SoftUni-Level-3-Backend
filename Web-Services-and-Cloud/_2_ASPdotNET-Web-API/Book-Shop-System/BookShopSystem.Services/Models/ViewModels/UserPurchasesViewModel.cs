namespace BookShopSystem.Services.Models.ViewModels
{
    using System.Collections.Generic;

    public class UserPurchasesViewModel
    {
        public string UserName { get; set; }

        public IEnumerable<PurchaseViewModel> Purchases { get; set; }
    }
}