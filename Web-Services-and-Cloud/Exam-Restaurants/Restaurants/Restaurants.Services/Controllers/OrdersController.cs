namespace Restaurants.Services.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using Restaurants.Models;
    using Restaurants.Services.Models.ViewModels;

    [RoutePrefix("api/orders")]
    [Authorize]
    public class OrdersController : BaseApiController
    {
        // GET /api/orders?startPage={start-page}&limit={page-size}&mealId={mealId}
        [HttpGet]
        public IHttpActionResult CreateOrder([FromUri]int startPage, [FromUri] int limit, [FromUri]int mealId)
        {
            var loggedUserId = this.UserIdProvider.GetUserId();
            if (loggedUserId == null)
            {
                return this.Unauthorized();
            }

            if (startPage < 0)
            {
                return this.BadRequest("Start page cannot be negative.");
            }

            if (limit < 0)
            {
                return this.BadRequest("Number of orders cannot be negative.");
            }

            var existingMeal = this.Data.Meals.All()
                .FirstOrDefault(m => m.Id == mealId);
            if (existingMeal == null)
            {
                return this.NotFound();
            }

            var orders = this.Data.Orders.All()
                .Where(o => o.MealId == mealId && o.OrderStatus == OrderStatus.Pending)
                .OrderBy(o => o.CreatedOn)
                .Skip(startPage * limit)
                .Take(limit)
                .Select(OrderViewModel.Create);

            return this.Ok(orders);
        }
    }
}