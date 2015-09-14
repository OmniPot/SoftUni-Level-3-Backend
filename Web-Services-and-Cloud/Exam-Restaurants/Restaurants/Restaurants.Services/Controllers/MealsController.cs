namespace Restaurants.Services.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using Restaurants.Models;
    using Restaurants.Services.Models.BindingModels;
    using Restaurants.Services.Models.ViewModels;

    [RoutePrefix("api/meals")]
    [Authorize]
    public class MealsController : BaseApiController
    {
        // POST /api/meals
        [HttpPost]
        public IHttpActionResult CreateNewMeal(MealBindingModel mealModel)
        {
            if (mealModel == null)
            {
                return this.BadRequest("Meal data cannot be null.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var existingRestaurant = this.Data.Restaurants.All()
                .FirstOrDefault(r => r.Id == mealModel.RestaurantId);
            if (existingRestaurant == null)
            {
                return this.NotFound();
            }

            var loggedUserId = this.UserIdProvider.GetUserId();
            if (loggedUserId == null)
            {
                return this.Unauthorized();
            }

            if (existingRestaurant.OwnerId != loggedUserId)
            {
                return this.BadRequest("Cannot create a meal in foreign restaurant.");
            }

            var existingMealType = this.Data.MealTypes.All()
                .FirstOrDefault(mt => mt.Id == mealModel.TypeId);
            if (existingMealType == null)
            {
                return this.NotFound();
            }

            var newMeal = new Meal
            {
                Name = mealModel.Name,
                Price = mealModel.Price,
                RestaurantId = mealModel.RestaurantId,
                Restaurant = existingRestaurant,
                TypeId = mealModel.TypeId,
                Type = existingMealType,
            };

            this.Data.Meals.Add(newMeal);
            existingRestaurant.Meals.Add(newMeal);

            this.Data.SaveChanges();

            return this.CreatedAtRoute(
                "DefaultApi",
                new
                {
                    Id = newMeal.Id,
                    Controller = "meals"
                },
                MealViewModel.CreateSingle(newMeal));
        }

        // PUT /api/meals/{mealId}
        [HttpPut]
        [Route("{mealId}")]
        public IHttpActionResult EditMeal(int mealId, MealEditBindingModel mealEditModel)
        {
            if (mealEditModel == null)
            {
                return this.BadRequest("Meal edit data cannot be null.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var existingMeal = this.Data.Meals.All()
                .FirstOrDefault(r => r.Id == mealId);
            if (existingMeal == null)
            {
                return this.NotFound();
            }

            var loggedUserId = this.UserIdProvider.GetUserId();
            if (existingMeal.Restaurant.OwnerId != loggedUserId)
            {
                return this.Unauthorized();
            }

            var existingMealType = this.Data.MealTypes.All()
                .FirstOrDefault(mt => mt.Id == mealEditModel.TypeId);
            if (existingMealType == null)
            {
                return this.NotFound();
            }

            existingMeal.Name = mealEditModel.Name;
            existingMeal.TypeId = mealEditModel.TypeId;
            existingMeal.Type = existingMealType;
            existingMeal.Price = mealEditModel.Price;

            this.Data.Meals.Update(existingMeal);
            this.Data.SaveChanges();

            var mealViewModel = MealViewModel.CreateSingle(existingMeal);

            return this.Ok(mealViewModel);
        }

        // DELETE /api/meals/{mealId}
        [HttpDelete]
        [Route("{mealId}")]
        public IHttpActionResult DeleteMeal(int mealId)
        {
            var existingMeal = this.Data.Meals.All()
                .FirstOrDefault(r => r.Id == mealId);
            if (existingMeal == null)
            {
                return this.NotFound();
            }

            var loggedUserId = this.UserIdProvider.GetUserId();
            if (existingMeal.Restaurant.OwnerId != loggedUserId)
            {
                return this.Unauthorized();
            }

            this.Data.Meals.Delete(existingMeal);
            this.Data.SaveChanges();

            return this.Ok(string.Format("Meal with Id {0} deleted.", existingMeal.Id));
        }

        // POST /api/meals/{mealId}/order
        [HttpPost]
        [Route("{mealId}/order")]
        public IHttpActionResult CreateOrder(int mealId, OrderBindingModel orderModel)
        {
            if (orderModel == null)
            {
                return this.BadRequest("Order data cannot be null.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var existingMeal = this.Data.Meals.All()
                .FirstOrDefault(r => r.Id == mealId);
            if (existingMeal == null)
            {
                return this.NotFound();
            }

            var loggedUserId = this.UserIdProvider.GetUserId();
            if (loggedUserId == null)
            {
                return this.Unauthorized();
            }

            var newOrder = new Order
            {
                Quantity = orderModel.Quantity,
                MealId = mealId,
                Meal = existingMeal,
                UserId = loggedUserId,
                CreatedOn = DateTime.Now,
                OrderStatus = OrderStatus.Pending
            };

            this.Data.Orders.Add(newOrder);
            this.Data.SaveChanges();

            return this.Ok(string.Format("Order with Id {0} is now pending.", newOrder.Id));
        }
    }
}