namespace OnlineShop.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Repositories;
    using Models;
    using Moq;

    public class MockContainer
    {
        public MockContainer()
        {
            this.CategoryRepositoryMock = new Mock<IRepository<Category>>();
            this.AdTypeRepositoryMock = new Mock<IRepository<AdType>>();
            this.UserRepositoryMock = new Mock<IRepository<ApplicationUser>>();
            this.CategoryRepositoryMock = new Mock<IRepository<Category>>();
        }

        public Mock<IRepository<Ad>> AdRepositoryMock { get; set; }

        public Mock<IRepository<AdType>> AdTypeRepositoryMock { get; set; }

        public Mock<IRepository<ApplicationUser>> UserRepositoryMock { get; set; }

        public Mock<IRepository<Category>> CategoryRepositoryMock { get; set; }

        public void PrepareMocks()
        {
            this.SetupFakeCategories();
            this.SetupFakeUsers();
            this.SetupFakeAdTypes();
            this.SetupFakeAds();
        }

        private void SetupFakeCategories()
        {
            var fakeCategories = new List<Category>
            {
                new Category {Id = 1, Name = "cars"},
                new Category {Id = 2, Name = "phones"},
                new Category {Id = 3, Name = "machines"},
                new Category {Id = 4, Name = "electronics"}
            };

            this.CategoryRepositoryMock.Setup(r => r.All()).Returns(fakeCategories.AsQueryable());
            this.CategoryRepositoryMock.Setup(r => r.Find(It.IsAny<int>()))
                .Returns((int id) => { return fakeCategories.FirstOrDefault(c => c.Id == id); });
        }

        private void SetupFakeUsers()
        {
            var fakeUsers = new List<ApplicationUser>
            {
                new ApplicationUser {UserName = "Iliq", Id = "123"},
                new ApplicationUser {UserName = "Stamen", Id = "456"}
            };

            this.UserRepositoryMock.Setup(r => r.All()).Returns(fakeUsers.AsQueryable());
            this.UserRepositoryMock.Setup(r => r.Find(It.IsAny<string>()))
                .Returns((string id) => { return fakeUsers.FirstOrDefault(u => u.Id == id); });
        }

        private void SetupFakeAdTypes()
        {
            var fakeAdTypes = new List<AdType>
            {
                new AdType {Name = "Normal", Index = 100},
                new AdType {Name = "Premium", Index = 200}
            };

            this.AdTypeRepositoryMock.Setup(r => r.All()).Returns(fakeAdTypes.AsQueryable());
            this.AdTypeRepositoryMock.Setup(r => r.Find(It.IsAny<int>()))
                .Returns((int id) => { return fakeAdTypes.FirstOrDefault(at => at.Id == id); });
        }

        private void SetupFakeAds()
        {
            this.AdRepositoryMock = new Mock<IRepository<Ad>>();

            var fakeAdTypes = new List<AdType>
            {
                new AdType {Name = "Normal", Index = 100},
                new AdType {Name = "Premium", Index = 200}
            };

            var fakeAds = new List<Ad>
            {
                new Ad
                {
                    Id = 1,
                    Name = "Audi A6",
                    Type = fakeAdTypes[1],
                    Status = AdStatus.Open,
                    PostedOn = DateTime.Now.AddDays(-6),
                    Owner = new ApplicationUser {Id = "456", UserName = "Gancho"},
                    Price = 5000
                },
                new Ad
                {
                    Id = 2,
                    Name = "Washing machine Deluxe",
                    Type = fakeAdTypes[0],
                    Status = AdStatus.Open,
                    PostedOn = DateTime.Now.AddDays(-10),
                    Owner = new ApplicationUser {Id = "123", UserName = "Dancho"},
                    Price = 800
                }
            };

            this.AdRepositoryMock.Setup(r => r.All()).Returns(fakeAds.AsQueryable());
            this.AdRepositoryMock.Setup(r => r.Find(It.IsAny<int>()))
                .Returns((int id) => { return fakeAds.FirstOrDefault(a => a.Id == id); });
        }
    }
}