namespace News.WebServices.Tests.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Web.Http;
    using Controllers;
    using Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using Moq;
    using News.Models;

    [TestClass]
    public class NewsControllerTests
    {
        private Mock<INewsData> mockContext;
        private MockContainer mocks;
        private NewsController newsController;

        [TestInitialize]
        public void InitTests()
        {
            this.mocks = new MockContainer();
            this.mocks.PrepareMocks();

            this.mockContext = new Mock<INewsData>();
            this.mockContext.Setup(c => c.News).Returns(this.mocks.NewsRepositoryMock.Object);

            this.newsController = new NewsController(this.mockContext.Object);
            this.ConfigureController(this.newsController);
        }


        [TestMethod]
        public void GetAllNews_Should_Return_Total_News_Sorted_By_PublishDate()
        {
            var fakeNews = this.mocks.NewsRepositoryMock.Object.All();

            this.mockContext.Setup(ctx => ctx.News.All()).Returns(fakeNews.AsQueryable());

            var response = this.newsController.GetAllNews().ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var newsResponse = response.Content.ReadAsAsync<IEnumerable<NewsViewModel>>().Result
                .Select(n => new
                {
                    n.Title
                }).ToList();

            var orderedFakeNews = fakeNews
                .OrderByDescending(a => a.PublishDate)
                .Select(a => new { a.Title })
                .ToList();

            CollectionAssert.AreEqual(newsResponse, orderedFakeNews);
        }

        [TestMethod]
        public void CreateNews_Should_Successfully_Add_To_Repository()
        {
            var fakeNews = new List<News>();

            this.mocks.NewsRepositoryMock.Setup(r => r.Add(It.IsAny<News>()))
                .Callback((News news) => { fakeNews.Add(news); });

            var newsToAdd = new NewsBindingModel
            {
                Title = "News to add Title.",
                Content = "News to add Content.",
                PublishDate = DateTime.Now.AddDays(-3)
            };

            var response = this.newsController.CreateNews(newsToAdd)
                .ExecuteAsync(CancellationToken.None).Result;
            var createdNews = response.Content.ReadAsAsync<News>().Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(createdNews.Title, newsToAdd.Title);
            Assert.AreEqual(createdNews.Content, newsToAdd.Content);

            this.mockContext.Verify(c => c.SaveChanges(), Times.Once);
            Assert.AreEqual(1, fakeNews.Count);
        }

        [TestMethod]
        public void CreateNews_Should_Return_400BadRequest_If_Data_Is_Invalid()
        {
            var fakeNews = new List<News>();

            this.mocks.NewsRepositoryMock.Setup(r => r.Add(It.IsAny<News>()))
                .Callback((News news) => { fakeNews.Add(news); });

            var newsToAdd = new NewsBindingModel
            {
                Title = "News",
                PublishDate = DateTime.Now.AddDays(-3)
            };

            this.newsController.Validate(newsToAdd);

            var response = this.newsController.CreateNews(newsToAdd)
                .ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            this.mockContext.Verify(c => c.SaveChanges(), Times.Never);
            Assert.AreEqual(0, fakeNews.Count);
        }

        [TestMethod]
        public void UpdateNews_Should_Successfully_Update_News()
        {
            var fakeNews = this.mocks.NewsRepositoryMock.Object.All().ToList();

            this.mocks.NewsRepositoryMock.Setup(r => r.Update(It.IsAny<News>()))
                .Callback((News news) => { fakeNews[0] = news; });

            var newsToUpdate = fakeNews.FirstOrDefault();
            if (newsToUpdate == null)
            {
                Assert.Fail("Cannot perform test - No news in the repository.");
            }

            var updatedNews = new NewsBindingModel
            {
                Title = "UPDATED NEWS.",
                Content = "UPDATED NEWS CONTENT.",
                PublishDate = DateTime.Now.AddDays(-1)
            };

            var response = this.newsController.UpdateNews(newsToUpdate.Id, updatedNews)
                .ExecuteAsync(CancellationToken.None).Result;
            var updateResultContent = response.Content.ReadAsAsync<NewsViewModel>().Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(newsToUpdate.Title, updateResultContent.Title);
            Assert.AreEqual(newsToUpdate.Content, updateResultContent.Content);

            this.mockContext.Verify(c => c.SaveChanges(), Times.Once);
            Assert.AreEqual(2, fakeNews.Count);
        }

        [TestMethod]
        public void UpdateNews_Should_Return_400BadRequest_If_Data_Is_Invalid()
        {
            var fakeNews = this.mocks.NewsRepositoryMock.Object.All().ToList();

            this.mocks.NewsRepositoryMock.Setup(r => r.Update(It.IsAny<News>()))
                .Callback((News news) => { fakeNews[0] = news; });

            var newsToUpdate = fakeNews.FirstOrDefault();
            if (newsToUpdate == null)
            {
                Assert.Fail("Cannot perform test - No news in the repository.");
            }

            var updatedNews = new NewsBindingModel
            {
                Content = "News",
                PublishDate = DateTime.Now.AddDays(-1)
            };

            this.newsController.Validate(updatedNews);

            var response = this.newsController.UpdateNews(newsToUpdate.Id, updatedNews)
                .ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual(newsToUpdate.Title, newsToUpdate.Title);
            Assert.AreEqual(newsToUpdate.Content, newsToUpdate.Content);

            this.mockContext.Verify(c => c.SaveChanges(), Times.Never);
        }

        [TestMethod]
        public void DeleteNews_Should_Successfully_Delete_News()
        {
            var fakeNews = this.mocks.NewsRepositoryMock.Object.All().ToList();

            this.mocks.NewsRepositoryMock.Setup(r => r.Delete(It.IsAny<News>()))
                .Callback((News news) => { fakeNews.Remove(news); });

            var newsToDelete = fakeNews.FirstOrDefault();
            if (newsToDelete == null)
            {
                Assert.Fail("Cannot perform test - No news in the repository.");
            }

            var response = this.newsController.DeleteNews(newsToDelete.Id)
                .ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            this.mockContext.Verify(c => c.SaveChanges(), Times.Once);
            Assert.AreEqual(1, fakeNews.Count);
            Assert.IsFalse(fakeNews.Contains(newsToDelete));
        }

        [TestMethod]
        public void DeleteNews_Should__Return_400BadRequest_If_Specified_Id_Is_Invalid()
        {
            var fakeNews = this.mocks.NewsRepositoryMock.Object.All().ToList();

            this.mocks.NewsRepositoryMock.Setup(r => r.Delete(It.IsAny<News>()))
                .Callback((News news) => { fakeNews.Remove(news); });

            var newsToDelete = new News
            {
                Id = 5
            };

            var response = this.newsController.DeleteNews(newsToDelete.Id)
                .ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            this.mockContext.Verify(c => c.SaveChanges(), Times.Never);
            Assert.AreEqual(2, fakeNews.Count);
        }

        private void ConfigureController(NewsController controller)
        {
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
        }
    }
}