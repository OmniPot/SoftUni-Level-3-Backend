namespace OnlineShop.Tests.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Web.Http;
    using Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using Moq;
    using Services.Controllers;
    using Services.Infrastructure;
    using Services.Models.BindingModels;
    using Services.Models.ViewModels;

    [TestClass]
    public class AdsControllerTests
    {
        private MockContainer mocks;

        [TestInitialize]
        public void InitTests()
        {
            this.mocks = new MockContainer();
            this.mocks.PrepareMocks();
        }

        [TestMethod]
        public void GetallAds_Should_Return_Total_Ads_Sorted_By_TypeIndex()
        {
            var fakeAds = this.mocks.AdRepositoryMock.Object.All();
            var mockContext = new Mock<IOnlineShopData>();
            mockContext.Setup(ctx => ctx.Ads.All()).Returns(fakeAds.AsQueryable());

            var adsController = new AdsController(mockContext.Object);
            this.ConfigureController(adsController);
            var response = adsController.All().ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var adsResponse = response.Content.ReadAsAsync<IEnumerable<AdViewModel>>().Result
                .Select(a => new
                {
                    a.Id
                }).ToList();

            var orderedFakeAds = fakeAds
                .OrderByDescending(a => a.Type.Index)
                .ThenByDescending(a => a.PostedOn)
                .Select(a => new { a.Id })
                .ToList();

            CollectionAssert.AreEqual(adsResponse, orderedFakeAds);
        }

        [TestMethod]
        public void CreateAd_Should_Successfully_Add_To_Repository()
        {
            var ads = new List<Ad>();

            var fakeUser = this.mocks.UserRepositoryMock.Object.All()
                .FirstOrDefault();
            if (fakeUser == null)
            {
                Assert.Fail("Cannot perform test - no users available.");
            }

            this.mocks.AdRepositoryMock.Setup(r => r.Add(It.IsAny<Ad>()))
                .Callback((Ad ad) =>
                {
                    ad.Owner = fakeUser;
                    ads.Add(ad);
                });

            var mockContext = new Mock<IOnlineShopData>();
            mockContext.Setup(c => c.Ads)
                .Returns(this.mocks.AdRepositoryMock.Object);
            mockContext.Setup(c => c.AdTypes)
                .Returns(this.mocks.AdTypeRepositoryMock.Object);
            mockContext.Setup(c => c.Users)
                .Returns(this.mocks.UserRepositoryMock.Object);
            mockContext.Setup(c => c.Categories)
                .Returns(this.mocks.CategoryRepositoryMock.Object);

            var mockIdProvider = new Mock<IUserIdProvider>();
            mockIdProvider.Setup(ip => ip.GetUserId())
                .Returns(fakeUser.Id);

            var adsController = new AdsController(mockContext.Object, mockIdProvider.Object);
            this.ConfigureController(adsController);

            var randomName = Guid.NewGuid().ToString();
            var newAd = new AdBindingModel
            {
                Name = randomName,
                Price = 555,
                TypeId = mockContext.Object.AdTypes.All().FirstOrDefault().Id,
                Description = "Nothing much to say",
                Categories = new[] { 1, 2, 3 }
            };

            var response = adsController.Create(newAd)
                .ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            mockContext.Verify(c => c.SaveChanges(), Times.Once);
            Assert.AreEqual(1, ads.Count);
            Assert.AreEqual(randomName, ads[0].Name);
        }

        [TestMethod]
        public void Closing_Ad_As_Owner_Should_Return_200OK()
        {
            var fakeAds = this.mocks.AdRepositoryMock.Object.All();
            var openAd = fakeAds.FirstOrDefault(ad => ad.Status == AdStatus.Open);

            if (openAd == null)
            {
                Assert.Fail("Cannot perform test - no open ads available.");
            }

            var openAdId = openAd.Id;

            var mockContext = new Mock<IOnlineShopData>();
            mockContext.Setup(c => c.Ads)
                .Returns(this.mocks.AdRepositoryMock.Object);

            var mockIdProvider = new Mock<IUserIdProvider>();
            mockIdProvider.Setup(p => p.GetUserId())
                .Returns(openAd.OwnerId);

            var adsController = new AdsController(mockContext.Object, mockIdProvider.Object);
            this.ConfigureController(adsController);

            var response = adsController.Close(openAdId).ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            mockContext.Verify(c => c.SaveChanges(), Times.Once);
            Assert.AreNotEqual(openAd.ClosedOn, null);
            Assert.AreEqual(openAd.Status, AdStatus.Closed);
        }

        [TestMethod]
        public void Close_Ad_As_NonOwner_Should_Return_400BadRequest()
        {
            var fakeAds = this.mocks.AdRepositoryMock.Object.All();
            var fakeUsers = this.mocks.UserRepositoryMock.Object.All();

            var openAd = fakeAds.FirstOrDefault(a => a.Status == AdStatus.Open);
            if (openAd == null)
            {
                Assert.Fail("Cannot perform test - no open ads exist.");
            }

            var openAdOwnerId = openAd.OwnerId;
            var fakeForeignUser = fakeUsers.FirstOrDefault(u => u.Id != openAdOwnerId);
            if (fakeForeignUser == null)
            {
                Assert.Fail("Cannot perform test - no foreign user available.");
            }

            var fakeForeignUserId = fakeForeignUser.Id;
            var openAdId = openAd.Id;

            var mockIdProvider = new Mock<IUserIdProvider>();
            mockIdProvider.Setup(ip => ip.GetUserId())
                .Returns(fakeForeignUserId);

            var mockContext = new Mock<IOnlineShopData>();
            mockContext.Setup(c => c.Ads).Returns(this.mocks.AdRepositoryMock.Object);

            var adsController = new AdsController(mockContext.Object, mockIdProvider.Object);
            this.ConfigureController(adsController);

            var response = adsController.Close(openAdId).ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            mockContext.Verify(c => c.SaveChanges(), Times.Never);
            Assert.AreEqual(AdStatus.Open, openAd.Status);
        }

        private void ConfigureController(AdsController controller)
        {
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
        }
    }
}