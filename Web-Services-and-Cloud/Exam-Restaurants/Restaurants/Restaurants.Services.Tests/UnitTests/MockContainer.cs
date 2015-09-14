namespace Restaurants.Services.Tests.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Moq;
    using Restaurants.Data.Repositories;
    using Restaurants.Models;

    public class MockContainer
    {
        public MockContainer()
        {
            this.UserRepositoryMock = new Mock<IRepository<ApplicationUser>>();
            this.TownRepositoryMock = new Mock<IRepository<Town>>();
            this.RestaurantRepositoryMock = new Mock<IRepository<Restaurant>>();
            this.RatingsRepositoryMock = new Mock<IRepository<Rating>>();
        }

        public Mock<IRepository<ApplicationUser>> UserRepositoryMock { get; set; }

        public Mock<IRepository<Town>> TownRepositoryMock { get; set; }

        public Mock<IRepository<Restaurant>> RestaurantRepositoryMock { get; set; }

        public Mock<IRepository<Rating>> RatingsRepositoryMock { get; set; }

        public void PrepareMocks()
        {
            this.SetupFakeUsers();
            this.SetupFakeTowns();
            this.SetupFakeRestaurants();
            this.SetupFakeRatings();
        }

        private void SetupFakeUsers()
        {
            var fakeTowns = new List<ApplicationUser>
            {
                new ApplicationUser {  Id = "123", UserName = "Stoqn"},
                new ApplicationUser {  Id = "123", UserName = "Gosho"}
            };

            this.UserRepositoryMock.Setup(r => r.All()).Returns(fakeTowns.AsQueryable());
            this.UserRepositoryMock.Setup(r => r.Find(It.IsAny<string>()))
                .Returns((string id) => { return fakeTowns.FirstOrDefault(u => u.Id == id); });
        }

        private void SetupFakeTowns()
        {
            var fakeTowns = new List<Town>
            {
                new Town {  Id = 1, Name = "Sofia"},
                new Town {  Id = 2, Name = "Varna"},
            };

            this.TownRepositoryMock.Setup(r => r.All()).Returns(fakeTowns.AsQueryable());
            this.TownRepositoryMock.Setup(r => r.Find(It.IsAny<string>()))
                .Returns((int id) => { return fakeTowns.FirstOrDefault(u => u.Id == id); });
        }

        private void SetupFakeRestaurants()
        {
            var fakeRestaurants = new List<Restaurant>
            {
                new Restaurant {  Id = 1, Name = "alabala", TownId = 1},
                new Restaurant {  Id = 2, Name = "portokala", TownId = 2},
            };

            this.RestaurantRepositoryMock.Setup(r => r.All()).Returns(fakeRestaurants.AsQueryable());
            this.RestaurantRepositoryMock.Setup(r => r.Find(It.IsAny<string>()))
                .Returns((int id) => { return fakeRestaurants.FirstOrDefault(u => u.Id == id); });
        }

        private void SetupFakeRatings()
        {
            var fakeRestaurants = new List<Rating>
            {
                new Rating {  Stars = 5, RestaurantId = 1, },
                new Rating {  Stars = 7, RestaurantId = 1, },
                new Rating {  Stars = 1, RestaurantId = 2, },
                new Rating {  Stars = 3, RestaurantId = 2, },
            };

            this.RatingsRepositoryMock.Setup(r => r.All()).Returns(fakeRestaurants.AsQueryable());
        }
    }
}