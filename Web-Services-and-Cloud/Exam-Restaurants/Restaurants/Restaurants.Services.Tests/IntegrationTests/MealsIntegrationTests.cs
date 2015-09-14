namespace Restaurants.Services.Tests.IntegrationTests
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Restaurants.Data;
    using Restaurants.Services.Tests.Models;

    [TestClass]
    public class MealsIntegrationTests
    {
        [TestMethod]
        public void EditMeal_WitInvalidEditData_ShouldReturn400BadRequest()
        {
            var context = new RestaurantsContext();

            string firstUserUsername = "petar" + DateTime.Now.Ticks;
            string firstUserPassword = "123456" + DateTime.Now.Ticks;
            string firstUserEmail = DateTime.Now.Ticks + "@pe.tar";

            var registerHttpResponse = TestingEngine.RegisterUserHttpPost(firstUserUsername, firstUserPassword, firstUserEmail);
            Assert.AreEqual(HttpStatusCode.OK, registerHttpResponse.StatusCode);

            var loginUserData = TestingEngine.LoginUser(firstUserUsername, firstUserPassword);
            Assert.AreEqual(loginUserData.UserName, firstUserUsername);

            string restaurantName = "V shubraka";
            int restaurantTownId = context.Towns.First().Id;

            var createRestaurantHttpResponse = TestingEngine.CreateRestaurantHttpPost(
                loginUserData.Access_Token, restaurantName, restaurantTownId);
            Assert.AreEqual(HttpStatusCode.Created, createRestaurantHttpResponse.StatusCode);
            var createdRestaurant = createRestaurantHttpResponse.Content.ReadAsAsync<RestaurantModel>().Result;

            var mealName = "Qdene" + DateTime.Now.Ticks;
            var mealType = 1;
            var mealPrice = 20m;
            var mealRestaurantId = createdRestaurant.Id;

            var createMealHttpPost = TestingEngine.CreateMealHttpPost(loginUserData.Access_Token, mealName, mealType, mealRestaurantId, mealPrice);
            var createdMeal = createMealHttpPost.Content.ReadAsAsync<MealModel>().Result;

            var putResponse1 = TestingEngine.EditMealHttpPut(loginUserData.Access_Token, createdMeal.Id, null, 3, 70m);
            Assert.AreEqual(HttpStatusCode.BadRequest, putResponse1.StatusCode);

            var putResponse2 = TestingEngine.EditMealHttpPut(loginUserData.Access_Token, createdMeal.Id, "mandja", null, 50m);
            Assert.AreEqual(HttpStatusCode.BadRequest, putResponse2.StatusCode);

            var putResponse3 = TestingEngine.EditMealHttpPut(loginUserData.Access_Token, createdMeal.Id, "fasul", 3, null);
            Assert.AreEqual(HttpStatusCode.BadRequest, putResponse3.StatusCode);
        }

        [TestMethod]
        public void EditMeal_NotOwnerUser_ShouldReturn401Unauthorized()
        {
            var context = new RestaurantsContext();

            string firstUserUsername = "petar" + DateTime.Now.Ticks;
            string firstUserPassword = "123456" + DateTime.Now.Ticks;
            string firstUserEmail = DateTime.Now.Ticks + "@pe.tar";

            string secondUserUsername = "iliant" + DateTime.Now.Ticks;
            string secondUserPassword = "123456" + DateTime.Now.Ticks;
            string secondUserEmail = DateTime.Now.Ticks + "@sa.sho";

            var registerFirstUserHttpResponse = TestingEngine.RegisterUserHttpPost(firstUserUsername, firstUserPassword, firstUserEmail);
            Assert.AreEqual(HttpStatusCode.OK, registerFirstUserHttpResponse.StatusCode);

            var registerSecondUserHttpResponse = TestingEngine.RegisterUserHttpPost(secondUserUsername, secondUserPassword, secondUserEmail);
            Assert.AreEqual(HttpStatusCode.OK, registerSecondUserHttpResponse.StatusCode);

            var loggedUserData = TestingEngine.LoginUser(firstUserUsername, firstUserPassword);
            Assert.AreEqual(loggedUserData.UserName, firstUserUsername);

            string restaurantName = "V shubraka";
            int restaurantTownId = context.Towns.First().Id;

            var createRestaurantHttpResponse = TestingEngine.CreateRestaurantHttpPost(
                loggedUserData.Access_Token, restaurantName, restaurantTownId);
            Assert.AreEqual(HttpStatusCode.Created, createRestaurantHttpResponse.StatusCode);
            var createdRestaurant = createRestaurantHttpResponse.Content.ReadAsAsync<RestaurantModel>().Result;

            var mealName = "Leleeeee" + DateTime.Now.Ticks;
            var mealType = 2;
            var mealPrice = 100m;
            var mealRestaurantId = createdRestaurant.Id;

            var createMealHttpPost = TestingEngine.CreateMealHttpPost(loggedUserData.Access_Token, mealName, mealType, mealRestaurantId, mealPrice);
            var createdMeal = createMealHttpPost.Content.ReadAsAsync<MealModel>().Result;

            loggedUserData = TestingEngine.LoginUser(secondUserUsername, secondUserPassword);
            Assert.AreEqual(loggedUserData.UserName, secondUserUsername);

            var putResponse = TestingEngine.EditMealHttpPut(loggedUserData.Access_Token, createdMeal.Id, "Hapvanka", 1, 200m);
            Assert.AreEqual(HttpStatusCode.Unauthorized, putResponse.StatusCode);
        }

        [TestMethod]
        public void EditMeal_NonExistingMeal_ShouldReturn404NotFound()
        {
            var context = new RestaurantsContext();

            string firstUserUsername = "petar" + DateTime.Now.Ticks;
            string firstUserPassword = "123456" + DateTime.Now.Ticks;
            string firstUserEmail = DateTime.Now.Ticks + "@pe.tar";

            var registerFirstUserHttpResponse = TestingEngine.RegisterUserHttpPost(firstUserUsername, firstUserPassword, firstUserEmail);
            Assert.AreEqual(HttpStatusCode.OK, registerFirstUserHttpResponse.StatusCode);

            var loggedUserData = TestingEngine.LoginUser(firstUserUsername, firstUserPassword);
            Assert.AreEqual(loggedUserData.UserName, firstUserUsername);

            string restaurantName = "V shubraka";
            int restaurantTownId = context.Towns.First().Id;

            var createRestaurantHttpResponse = TestingEngine.CreateRestaurantHttpPost(
                loggedUserData.Access_Token, restaurantName, restaurantTownId);
            Assert.AreEqual(HttpStatusCode.Created, createRestaurantHttpResponse.StatusCode);
            var createdRestaurant = createRestaurantHttpResponse.Content.ReadAsAsync<RestaurantModel>().Result;

            var mealName = "Leleeeee" + DateTime.Now.Ticks;
            var mealType = 2;
            var mealPrice = 100m;
            var mealRestaurantId = createdRestaurant.Id;

            TestingEngine.CreateMealHttpPost(loggedUserData.Access_Token, mealName, mealType, mealRestaurantId, mealPrice);

            var putResponse = TestingEngine.EditMealHttpPut(loggedUserData.Access_Token, 0, "Hapvanka", 1, 200m);
            Assert.AreEqual(HttpStatusCode.NotFound, putResponse.StatusCode);
        }

        [TestMethod]
        public void EditMeal_ShouldReturn200Ok()
        {
            var context = new RestaurantsContext();

            string firstUserUsername = "petar" + DateTime.Now.Ticks;
            string firstUserPassword = "123456" + DateTime.Now.Ticks;
            string firstUserEmail = DateTime.Now.Ticks + "@pe.tar";

            var registerHttpResponse = TestingEngine.RegisterUserHttpPost(firstUserUsername, firstUserPassword, firstUserEmail);
            Assert.AreEqual(HttpStatusCode.OK, registerHttpResponse.StatusCode);

            var loginUserData = TestingEngine.LoginUser(firstUserUsername, firstUserPassword);
            Assert.AreEqual(loginUserData.UserName, firstUserUsername);

            string restaurantName = "V shubraka";
            int restaurantTownId = context.Towns.First().Id;

            var createRestaurantHttpResponse = TestingEngine.CreateRestaurantHttpPost(
                loginUserData.Access_Token, restaurantName, restaurantTownId);
            Assert.AreEqual(HttpStatusCode.Created, createRestaurantHttpResponse.StatusCode);
            var createdRestaurant = createRestaurantHttpResponse.Content.ReadAsAsync<RestaurantModel>().Result;

            var mealName = "Qdene" + DateTime.Now.Ticks;
            var mealType = 1;
            var mealPrice = 20m;
            var mealRestaurantId = createdRestaurant.Id;

            var createMealHttpPost = TestingEngine.CreateMealHttpPost(loginUserData.Access_Token, mealName, mealType, mealRestaurantId, mealPrice);
            var createdMeal = createMealHttpPost.Content.ReadAsAsync<MealModel>().Result;

            var putResponse = TestingEngine.EditMealHttpPut(loginUserData.Access_Token, createdMeal.Id, "Success", 3, 70m);
            Assert.AreEqual(HttpStatusCode.OK, putResponse.StatusCode);
        }
    }
}
