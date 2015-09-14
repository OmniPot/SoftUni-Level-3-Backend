namespace BookShopSystem.Services.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using BookShopSystem.Models;
    using Data;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models.ViewModels;

    [RoutePrefix("api/user")]
    public class UsersController : BaseApiController
    {
        private readonly ApplicationUserManager userManager;

        public UsersController()
        {
            this.userManager = new ApplicationUserManager(
                new UserStore<User>(new BookShopSystemDbContext()));
        }

        public ApplicationUserManager UserManager
        {
            get { return this.userManager; }
        }

        [Authorize]
        [HttpGet]
        [Route("{username}/purchases")]
        public IHttpActionResult GetUserPurchases(string username)
        {
            var existingUser = this.BookShopData.Users.All()
                .FirstOrDefault(u => u.UserName.Equals(username));

            if (existingUser == null)
            {
                return this.NotFound();
            }

            var userPurchases = new UserPurchasesViewModel
            {
                UserName = existingUser.UserName,
                Purchases = existingUser.Purchases
                    .AsQueryable()
                    .Where(p => !p.isRecalled)
                    .OrderByDescending(p => p.DateOfPurchase)
                    .Select(PurchaseViewModel.GetViewModel)
            };

            return this.Ok(userPurchases);
        }
    }
}
