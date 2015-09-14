namespace News.WebServices.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Repositories;
    using Moq;
    using News.Models;

    public class MockContainer
    {
        public MockContainer()
        {
            this.NewsRepositoryMock = new Mock<IRepository<News>>();
        }

        public Mock<IRepository<News>> NewsRepositoryMock { get; set; }

        public void PrepareMocks()
        {
            this.SetupFakeNews();
        }

        private void SetupFakeNews()
        {
            var fakeNews = new List<News>
            {
                new News
                {
                    Id = 1,
                    Title = "fake news",
                    Content = "bla bla bla",
                    PublishDate = DateTime.Now.AddDays(-1)
                },
                new News
                {
                    Id = 2,
                    Title = "fake news 2",
                    Content = "sha lq lq",
                    PublishDate = DateTime.Now.AddDays(-3)
                }
            };

            this.NewsRepositoryMock.Setup(r => r.All()).Returns(fakeNews.AsQueryable());
            this.NewsRepositoryMock.Setup(r => r.Find(It.IsAny<int>()))
                .Returns((int id) => { return fakeNews.FirstOrDefault(c => c.Id == id); });
        }
    }
}