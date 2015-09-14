namespace News.WebServices.Tests.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Data;
    using EntityFramework.Extensions;
    using Microsoft.Owin.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using News.Models;
    using Owin;

    [TestClass]
    public class NewsIntegrationTests
    {
        private static TestServer httpTestServer;
        private static HttpClient httpClient;

        private const string GetAllNewsEndpoint = "api/news";
        private const string CreateNewsEndpoint = "api/news";
        private const string UpdateNewsEndpoint = "api/news/";
        private const string DeleteNewsEndpoint = "api/news/";

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
            var context = new NewsContext();

            CleanDatabase();

            SeedNews(context);

            context.SaveChanges();
        }

        private static void CleanDatabase()
        {
            var context = new NewsContext();

            context.News.Delete();
        }

        private static void SeedNews(NewsContext context)
        {
            if (context.News.Any())
            {
                return;
            }

            var testNews = new List<News>()
            {
                new News { Title = "First news title!", Content = "First news content!", PublishDate = DateTime.Now },
                new News { Title = "Second news title!", Content = "Second news content!" , PublishDate = DateTime.Now },
                new News { Title = "Third news title!", Content = "Third news content!", PublishDate = DateTime.Now },
                new News { Title = "Fourth news title!", Content = "Fourth news content!", PublishDate = DateTime.Now }
            };

            foreach (var news in testNews)
            {
                context.News.Add(news);
            }
        }

        [TestMethod]
        public void GetNews_Should_Return_200OK_And_News_As_JSON()
        {
            var response = httpClient.GetAsync(GetAllNewsEndpoint).Result;
            var news = response.Content.ReadAsAsync<IEnumerable<NewsViewModel>>().Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(4, news.Count());
        }

        [TestMethod]
        public void CreateNews_Should_Return_200OK_And_The_Created_News()
        {
            var context = new NewsContext();

            var newNewsData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string> ("Title", "First title"),
                new KeyValuePair<string, string> ("Content", "First contet"),
                new KeyValuePair<string, string> ("PublishDate", DateTime.Now.AddDays(-1).ToShortDateString())
            });

            var initialCount = context.News.Count();
            var response = httpClient.PostAsync(CreateNewsEndpoint, newNewsData).Result;
            var updatedCount = context.News.Count();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(initialCount + 1, updatedCount);
        }

        [TestMethod]
        public void CreateNews_Should_Return_400_BadRequest_If_Data_Is_Invalid()
        {
            var context = new NewsContext();

            var newNewsData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string> ("Title", "First title"),
                new KeyValuePair<string, string> ("PublishDate", DateTime.Now.AddDays(-1).ToShortDateString())
            });

            var initialCount = context.News.Count();
            var response = httpClient.PostAsync(CreateNewsEndpoint, newNewsData).Result;
            var updatedCount = context.News.Count();

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual(initialCount, updatedCount);
        }

        [TestMethod]
        public void ModifyNews_Should_Return_200OK_And_Updated_News()
        {
            var context = new NewsContext();
            var newsToUpdate = context.News.First();

            var newNewsData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string> ("Title", "First title"),
                new KeyValuePair<string, string> ("Content", "First content"),
            });

            var updateNewsResponse = httpClient.PutAsync(UpdateNewsEndpoint + newsToUpdate.Id, newNewsData).Result;
            var updatedNews = updateNewsResponse.Content.ReadAsAsync<News>().Result;

            Assert.AreEqual(HttpStatusCode.OK, updateNewsResponse.StatusCode);
            Assert.AreEqual("First title", updatedNews.Title);
            Assert.AreEqual("First content", updatedNews.Content);
        }

        [TestMethod]
        public void ModifyNews_Should_Return_400_BadRequest_If_Data_Is_Invalid()
        {
            var context = new NewsContext();
            var newsToUpdate = context.News.First();

            var newNewsData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string> ("Title", "First title"),
                new KeyValuePair<string, string> ("Content", "F")
            });

            var updateNewsResponse = httpClient.PutAsync(UpdateNewsEndpoint + newsToUpdate.Id, newNewsData).Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, updateNewsResponse.StatusCode);
        }

        [TestMethod]
        public void DeleteNews_Should_Return_200OK()
        {
            var context = new NewsContext();
            var newsToDeleteId = context.News.First().Id;
            var updateNewsResponse = httpClient.DeleteAsync(DeleteNewsEndpoint + newsToDeleteId).Result;

            Assert.AreEqual(HttpStatusCode.OK, updateNewsResponse.StatusCode);

            var getAllNews = httpClient.GetAsync(GetAllNewsEndpoint).Result;
            var returnedNewsId = getAllNews.Content.ReadAsAsync<List<News>>().Result
                .Select(n => n.Id)
                .ToList();

            CollectionAssert.DoesNotContain(returnedNewsId, newsToDeleteId);
        }

        [TestMethod]
        public void DeleteNews_Should_Return_400_BadRequest_If_News_Is_Invalid()
        {
            var newsToDeleteId = 0;
            var updateNewsResponse = httpClient.DeleteAsync(DeleteNewsEndpoint + newsToDeleteId).Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, updateNewsResponse.StatusCode);
        }
    }
}
