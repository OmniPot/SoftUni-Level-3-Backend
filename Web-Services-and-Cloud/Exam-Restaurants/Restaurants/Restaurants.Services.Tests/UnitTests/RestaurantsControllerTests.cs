namespace Restaurants.Services.Tests.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Web.Http;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Restaurants.Data.UnitOfWork;
    using Restaurants.Services.Controllers;
    using Restaurants.Services.Infrastructure;
    using Restaurants.Services.Models.ViewModels;
    using Restaurants.Services.Tests.Models;

    [TestClass]
    public class RestaurantsControllerTests
    {
        private RestaurantsController restaurantsController;
        private Mock<IRestaurantsData> mockContext;
        private Mock<IUserIdProvider> mockUserIdProvider;
        private MockContainer mocks;

        [TestInitialize]
        public void InitTests()
        {
            this.mocks = new MockContainer();
            this.mocks.PrepareMocks();

            this.mockContext = new Mock<IRestaurantsData>();
            this.ConfigureMockContext();

            this.mockUserIdProvider = new Mock<IUserIdProvider>();

            this.restaurantsController = new RestaurantsController(
                this.mockContext.Object, this.mockUserIdProvider.Object);
            this.ConfigureController(this.restaurantsController);
        }

        [TestMethod]
        public void GetRestaurants_ShouldReturn200OK_RestaurantsData()
        {
            // Arrange
            var fakeTownId = this.mocks.TownRepositoryMock.Object.All().First().Id;

            // Act
            var response = this.restaurantsController.GetRestaurantsByTown(fakeTownId).ExecuteAsync(CancellationToken.None).Result;
            var responseRestaurants = response.Content.ReadAsAsync<List<RestaurantModel>>().Result
                .Select(r => r.Id)
                .ToList();

            var expectedBids = this.mockContext.Object.Restaurants.All()
                .Where(r => r.TownId == fakeTownId)
                .OrderByDescending(r => r.Ratings.Average(rr => rr.Stars))
                .ThenBy(r => r.Name)
                .Select(b => b.Id)
                .ToList();

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            CollectionAssert.AreEqual(responseRestaurants, expectedBids);
        }

        private void ConfigureMockContext()
        {
            this.mockContext.Setup(c => c.Towns).Returns(this.mocks.TownRepositoryMock.Object);
            this.mockContext.Setup(c => c.Restaurants).Returns(this.mocks.RestaurantRepositoryMock.Object);
        }

        private void ConfigureController(RestaurantsController controller)
        {
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
        }
    }
}
