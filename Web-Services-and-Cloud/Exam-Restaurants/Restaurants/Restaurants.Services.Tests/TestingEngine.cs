namespace Restaurants.Services.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web.Http;
    using EntityFramework.Extensions;
    using Microsoft.Owin.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Owin;
    using Restaurants.Data;
    using Restaurants.Models;
    using Restaurants.Services.Tests.Models;

    [TestClass]
    public static class TestingEngine
    {
        private static TestServer TestWebServer { get; set; }

        public static HttpClient HttpClient { get; private set; }

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            // Start OWIN testing HTTP server with Web API support
            TestWebServer = TestServer.Create(appBuilder =>
            {
                var config = new HttpConfiguration();
                WebApiConfig.Register(config);
                var webAppStartup = new Startup();
                webAppStartup.Configuration(appBuilder);
                appBuilder.UseWebApi(config);
            });
            HttpClient = TestWebServer.HttpClient;
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            // Stop the OWIN testing HTTP server
            TestWebServer.Dispose();

            CleanDatabase();
        }

        public static void CleanDatabase()
        {
            var dbContext = new RestaurantsContext();

            dbContext.Meals.Delete();
            dbContext.Restaurants.Delete();
            dbContext.Towns.Delete();
            dbContext.Users.Delete();

            dbContext.SaveChanges();
        }

        public static HttpResponseMessage RegisterUserHttpPost(string username, string password, string email)
        {
            var postContent = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("confirmPassword", password),
                new KeyValuePair<string, string>("email", email)
            });
            var httpResponse = TestingEngine.HttpClient.PostAsync("api/account/register", postContent).Result;
            return httpResponse;
        }

        public static HttpResponseMessage LoginUserHttpPost(string username, string password)
        {
            var postContent = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("grant_type", "password")
            });
            var httpResponse = TestingEngine.HttpClient.PostAsync("api/account/login", postContent).Result;
            return httpResponse;
        }

        public static UserSessionModel LoginUser(string username, string password)
        {
            var httpResponse = TestingEngine.LoginUserHttpPost(username, password);
            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);
            var userSession = httpResponse.Content.ReadAsAsync<UserSessionModel>().Result;
            return userSession;
        }

        public static HttpResponseMessage CreateRestaurantHttpPost(string userSessionToken, string name, int? townId)
        {
            var postContent = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("name", name),
                new KeyValuePair<string, string>("townId", townId != null ?
                    townId.Value.ToString(CultureInfo.InvariantCulture) : null)
            });
            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("api/restaurants", UriKind.Relative),
                Content = postContent
            };
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userSessionToken);
            var httpResponse = TestingEngine.HttpClient.SendAsync(httpRequest).Result;
            return httpResponse;
        }

        public static HttpResponseMessage CreateMealHttpPost(string userSessionToken, string name, int? typeId, int? restaurantId, decimal? price)
        {
            var postContent = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("name", name),
                new KeyValuePair<string, string>("typeId", typeId != null ?
                    typeId.Value.ToString(CultureInfo.InvariantCulture) : null),
                new KeyValuePair<string, string>("restaurantId", restaurantId != null ?
                    restaurantId.Value.ToString(CultureInfo.InvariantCulture) : null),
            new KeyValuePair<string, string>("price", price != null ?
                    price.Value.ToString(CultureInfo.InvariantCulture) : null)
            });
            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("api/meals", UriKind.Relative),
                Content = postContent
            };
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userSessionToken);
            var httpResponse = TestingEngine.HttpClient.SendAsync(httpRequest).Result;
            return httpResponse;
        }

        public static HttpResponseMessage EditMealHttpPut(string userSessionToken, int mealId, string name, int? typeId, decimal? price)
        {
            var putContent = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("name", name),
                new KeyValuePair<string, string>("typeId", typeId != null ?
                    typeId.Value.ToString(CultureInfo.InvariantCulture) : null),
            new KeyValuePair<string, string>("price", price != null ?
                    price.Value.ToString(CultureInfo.InvariantCulture) : null)
            });
            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri("api/meals/" + mealId, UriKind.Relative),
                Content = putContent
            };
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userSessionToken);
            var httpResponse = TestingEngine.HttpClient.SendAsync(httpRequest).Result;
            return httpResponse;
        }
    }
}
