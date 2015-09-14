namespace Restaurants.Services.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using Restaurants.Data.UnitOfWork;
    using Restaurants.Models;
    using Restaurants.Services.Infrastructure;
    using Restaurants.Services.Models.BindingModels;
    using Restaurants.Services.Models.ViewModels;

    [RoutePrefix("api/restaurants")]
    [Authorize]
    public class RestaurantsController : BaseApiController
    {
        public RestaurantsController()
        {
        }

        public RestaurantsController(IRestaurantsData data, IUserIdProvider provider)
            : base(data, provider)
        {
        }

        // GET /api/restaurants?townId={townId}
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetRestaurantsByTown([FromUri] int townId)
        {
            var existingTown = this.Data.Towns.All()
                .FirstOrDefault(t => t.Id == townId);
            if (existingTown == null)
            {
                return this.NotFound();
            }

            var restaurantsByTown = this.Data.Restaurants.All()
                .Where(r => r.TownId == townId)
                .OrderByDescending(r => r.Ratings.Average(rr => rr.Stars))
                .ThenBy(r => r.Name)
                .Select(RestaurantViewModel.Create);

            return this.Ok(restaurantsByTown);
        }

        // POST /api/restaurants
        [HttpPost]
        public IHttpActionResult CreateRestaurant(RestaurantBindingModel restaurantModel)
        {
            var loggedUserId = this.UserIdProvider.GetUserId();
            if (loggedUserId == null)
            {
                return this.Unauthorized();
            }

            var loggedUser = this.Data.ApplicationUsers.All()
                .FirstOrDefault(u => u.Id == loggedUserId);

            if (restaurantModel == null)
            {
                return this.BadRequest();
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var existingTown = this.Data.Towns.All()
                .FirstOrDefault(t => t.Id == restaurantModel.TownId);
            if (existingTown == null)
            {
                return this.NotFound();
            }

            var newRestaurant = new Restaurant
            {
                Name = restaurantModel.Name,
                TownId = restaurantModel.TownId,
                Town = existingTown,
                OwnerId = loggedUserId,
                Owner = loggedUser
            };

            this.Data.Restaurants.Add(newRestaurant);
            existingTown.Restaurants.Add(newRestaurant);
            this.Data.SaveChanges();

            return this.CreatedAtRoute(
                "DefaultApi",
                new
                {
                    Id = newRestaurant.Id,
                    Controller = "restaurants"
                },
                RestaurantViewModel.CreateSingle(newRestaurant));
        }

        // POST /api/restaurants/{id}/rate
        [HttpPost]
        [Route("{restaurantId}/rate")]
        public IHttpActionResult RateRestaurant(int restaurantId, RatingBindingModel ratingModel)
        {
            if (ratingModel == null)
            {
                return this.BadRequest("Rating data cannot be null.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var loggedUserId = this.UserIdProvider.GetUserId();
            if (loggedUserId == null)
            {
                return this.Unauthorized();
            }

            var loggedUser = this.Data.ApplicationUsers.All()
                .FirstOrDefault(u => u.Id == loggedUserId);

            var existingRestaurant = this.Data.Restaurants.All()
                .FirstOrDefault(r => r.Id == restaurantId);
            if (existingRestaurant == null)
            {
                return this.NotFound();
            }

            if (existingRestaurant.OwnerId == loggedUserId)
            {
                return this.BadRequest("Restaurant owner cannot rate his own restaurant.");
            }

            if (loggedUser.GivenRatings.Any(r => r.RestaurantId == restaurantId))
            {
                return this.BadRequest("Already rated that restaurant.");
            }

            var rating = new Rating
            {
                Stars = ratingModel.Stars,
                RestaurantId = restaurantId,
                Restaurant = existingRestaurant,
                User = loggedUser,
                UserId = loggedUserId
            };

            this.Data.Ratings.Add(rating);

            existingRestaurant.Ratings.Add(rating);
            loggedUser.GivenRatings.Add(rating);

            this.Data.ApplicationUsers.Update(loggedUser);
            this.Data.Restaurants.Update(existingRestaurant);

            this.Data.SaveChanges();

            return this.Ok();
        }

        // GET /api/restaurants/{id}/meals
        [HttpGet]
        [AllowAnonymous]
        [Route("{restaurantId}/meals")]
        public IHttpActionResult GetRestaurantMeals(int restaurantId)
        {
            var existingRestaurant = this.Data.Restaurants.All()
                .FirstOrDefault(r => r.Id == restaurantId);
            if (existingRestaurant == null)
            {
                return this.NotFound();
            }

            var restaurantMeals = this.Data.Meals.All()
                .Where(m => m.RestaurantId == restaurantId)
                .Select(MealViewModel.Create);

            return this.Ok(restaurantMeals);
        }
    }
}