namespace BidSystem.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Web.Http;
    using BidSystem.Data.Contracts;
    using BidSystem.RestServices.Controllers;
    using BidSystem.RestServices.Infrastructure;
    using BidSystem.RestServices.Models.ViewModels;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class MyBidsUnitTestsWithMocking
    {
        private MockContainer mocks;
        private BidsController bidsController;

        private Mock<IBidsData> mockContext;
        private Mock<IUserIdProvider> mockUserIdProvider;

        [TestInitialize]
        public void InitTests()
        {
            this.mocks = new MockContainer();
            this.mocks.PrepareMocks();

            this.mockContext = new Mock<IBidsData>();
            this.ConfigureMockContext();

            this.mockUserIdProvider = new Mock<IUserIdProvider>();

            this.bidsController = new BidsController(
                this.mockContext.Object, this.mockUserIdProvider.Object);
            this.ConfigureController(this.bidsController);
        }

        [TestMethod]
        public void GetMyBids_Unauthorized_ShouldReturn401Unauthorized()
        {
            // Act
            var response = this.bidsController.UserBids()
                .ExecuteAsync(CancellationToken.None).Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public void GetMyBids_ShouldReturn200OK_BidsData()
        {
            // Arrange
            this.mockUserIdProvider = new Mock<IUserIdProvider>();
            this.ConfigureMockUserProvider();
            this.bidsController = new BidsController(this.mockContext.Object, this.mockUserIdProvider.Object);
            this.ConfigureController(this.bidsController);

            var fakeUserId = this.mockUserIdProvider.Object.GetUserId();

            // Act
            var response = this.bidsController.UserBids().ExecuteAsync(CancellationToken.None).Result;
            var responseBids = response.Content.ReadAsAsync<List<BidViewModel>>().Result
                .Select(b => b.Id)
                .ToList();

            var expectedBids = this.mockContext.Object.Bids.All()
                .Where(b => b.BidderId.Equals(fakeUserId))
                .OrderByDescending(b => b.BidDate)
                .Select(b => b.Id)
                .ToList();

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            CollectionAssert.AreEqual(responseBids, expectedBids);
        }

        private void ConfigureMockContext()
        {
            this.mockContext.Setup(c => c.Bids).Returns(this.mocks.BidsRepositoryMock.Object);
            this.mockContext.Setup(c => c.Users).Returns(this.mocks.UsersRepositoryMock.Object);
            this.mockContext.Setup(c => c.Offers).Returns(this.mocks.OffersRepositoryMock.Object);
        }

        private void ConfigureMockUserProvider()
        {
            this.mockUserIdProvider.Setup(ip => ip.GetUserId())
                .Returns(this.mocks.UsersRepositoryMock.Object.All().First().Id);
        }

        private void ConfigureController(BidsController controller)
        {
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
        }
    }
}
