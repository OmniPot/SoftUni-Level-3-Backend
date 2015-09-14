namespace OnlineShop.Tests.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Data;
    using EntityFramework.Extensions;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using Owin;
    using Services;

    [TestClass]
    public class AdsIntegrationTests
    {
        private const string TestUserUsername = "prakash";
        private const string TestUserPassword = "prakash";

        private static TestServer httpTestServer;
        private static HttpClient httpClient;

        private string accessToken;

        private string AccessToken
        {
            get
            {
                if (this.accessToken == null)
                {
                    var loginResponse = this.Login();
                    if (!loginResponse.IsSuccessStatusCode)
                    {
                        Assert.Fail("Unable to login: " + loginResponse.ReasonPhrase);
                    }

                    var loginData = loginResponse.Content
                        .ReadAsAsync<LoginDto>().Result;

                    this.accessToken = loginData.Access_Token;
                }

                return this.accessToken;
            }
        }

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            httpTestServer = TestServer.Create(appBuilder =>
            {
                var config = new HttpConfiguration();
                WebApiConfig.Register(config);
                var startup = new Startup();

                startup.Configuration(appBuilder);
                appBuilder.UseWebApi(config);
            });

            httpClient = httpTestServer.HttpClient;

            SeedDatabase();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            if (httpTestServer != null)
            {
                httpTestServer.Dispose();
            }

            CleanDatabase();
        }

        private static void SeedDatabase()
        {
            var context = new OnlineShopContext();

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new ApplicationUserManager(userStore);

            CleanDatabase();

            if (context.Users.FirstOrDefault(u => u.UserName == TestUserUsername) == null)
            {

                var user = new ApplicationUser
                {
                    UserName = TestUserUsername,
                    Email = "prakash@yahoo.in"
                };

                var result = userManager.CreateAsync(user,
                TestUserPassword).Result;
                if (!result.Succeeded)
                {
                    Assert.Fail(string.Join(Environment.NewLine, result.Errors));
                }
            }

            SeedCategories(context);
            SeedAdTypes(context);

            context.SaveChanges();
        }

        private static void CleanDatabase()
        {
            var context = new OnlineShopContext();

            context.Ads.Delete();
            context.AdTypes.Delete();
            context.Categories.Delete();
            context.Users.Delete();
        }

        private static void SeedAdTypes(OnlineShopContext context)
        {
            if (context.AdTypes.Any())
            {
                return;
            }

            var testAdTypes = new List<AdType>
            {
                new AdType {Name = "Normal", Index = 100},
                new AdType {Name = "Premium", Index = 200}
            };

            foreach (var type in testAdTypes)
            {
                context.AdTypes.Add(type);
            }
        }

        private static void SeedCategories(OnlineShopContext context)
        {
            if (context.Categories.Any())
            {
                return;
            }

            var testCategories = new List<Category>
            {
                new Category {Id = 1, Name = "cars"},
                new Category {Id = 2, Name = "phones"},
                new Category {Id = 3, Name = "machines"},
                new Category {Id = 4, Name = "electronics"}
            };

            foreach (var category in testCategories)
            {
                context.Categories.Add(category);
            }
        }

        private HttpResponseMessage Login()
        {
            var loginData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", TestUserUsername), 
                new KeyValuePair<string, string>("password", TestUserPassword), 
                new KeyValuePair<string, string>("grant_type", "password"), 
            });

            var response = httpClient.PostAsync("/Token", loginData).Result;

            return response;
        }

        private HttpResponseMessage PostNewAd(FormUrlEncodedContent data)
        {
            if (httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                httpClient.DefaultRequestHeaders.Remove("Authorization");
            }

            httpClient.DefaultRequestHeaders.Add("Authorization"
                , "Bearer " + this.AccessToken);

            return httpClient.PostAsync("api/ads/create", data).Result;
        }

        [TestMethod]
        public void Login_Should_Return_200OK_And_Access_Token()
        {
            var loginResponse = this.Login();

            Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);

            var loginData = loginResponse.Content
                .ReadAsAsync<LoginDto>().Result;

            Assert.IsNotNull(loginData.Access_Token);
        }

        [TestMethod]
        public void Posting_Ad_With_Invalid_AdType_Should_Return_Bad_Request()
        {
            var context = new OnlineShopContext();
            var category = context.Categories.First();

            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("name", "Opel Astra"), 
                new KeyValuePair<string, string>("description", "..."), 
                new KeyValuePair<string, string>("price", "2000"), 
                new KeyValuePair<string, string>("typeid", "-1"), 
                new KeyValuePair<string, string>("categories[0]", category.Id.ToString()), 
            });

            var response = this.PostNewAd(data);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void Posting_Ad_Without_Categories_Should_Return_Bad_Request()
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("name", "Opel Astra"), 
                new KeyValuePair<string, string>("description", "..."), 
                new KeyValuePair<string, string>("price", "2000"), 
                new KeyValuePair<string, string>("typeid", "1")
            });

            var response = this.PostNewAd(data);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void Posting_Ad_With_More_Than_3_Categories_Should_Return_Bad_Request()
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("name", "Opel Astra"), 
                new KeyValuePair<string, string>("description", "..."), 
                new KeyValuePair<string, string>("price", "2000"), 
                new KeyValuePair<string, string>("typeid", "1"),
                new KeyValuePair<string, string>("categories[0]", "1"),
                new KeyValuePair<string, string>("categories[1]", "2"),
                new KeyValuePair<string, string>("categories[2]", "3"),
                new KeyValuePair<string, string>("categories[3]", "4"),
            });

            var response = this.PostNewAd(data);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void Posting_Ad_Without_Name_Should_Return_Bad_Request()
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("description", "..."), 
                new KeyValuePair<string, string>("price", "2000"), 
                new KeyValuePair<string, string>("typeid", "1"),
                new KeyValuePair<string, string>("categories[0]", "1")
            });

            var response = this.PostNewAd(data);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void Posting_Valid_Ad_Should_Return_200OK_And_Create_Ad()
        {
            var context = new OnlineShopContext();
            var category = context.Categories.First();
            var adtype = context.AdTypes.First();

            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("name", "Mercedez Benz"), 
                new KeyValuePair<string, string>("description", "..."), 
                new KeyValuePair<string, string>("price", "2000"), 
                new KeyValuePair<string, string>("typeid", adtype.Id.ToString()),
                new KeyValuePair<string, string>("categories[0]", category.Id.ToString()),
            });

            var response = this.PostNewAd(data);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}