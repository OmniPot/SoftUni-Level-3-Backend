namespace BidSystem.Tests
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using BidSystem.Tests.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class OfferDetailsIntegrationTests
    {
        [TestMethod]
        public void GetOfferDetails_NonExistingOffer_ShouldReturn400NotFound()
        {
            // Arrange
            TestingEngine.CleanDatabase();

            // Act
            var nonExistingOfferId = 0;
            var url = string.Format("api/offers/{0}/details", nonExistingOfferId);
            var httpGetResponse = TestingEngine.HttpClient.GetAsync(url, CancellationToken.None).Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, httpGetResponse.StatusCode);
        }

        [TestMethod]
        public void GetOfferDetails_ExistingOffer_ShouldReturn200Ok_And_OfferDetails()
        {
            // Arrange
            TestingEngine.CleanDatabase();
            var userSession = TestingEngine.RegisterUser("peter", "pAssW@rd#123456");
            var offerToAdd = new OfferModel
            {
                Title = "First Offer",
                Description = "Description",
                InitialPrice = 200,
                ExpirationDateTime = DateTime.Now.AddDays(-5)
            };
            var httpCreateOfferResponse = TestingEngine.CreateOfferHttpPost(
                userSession.Access_Token,
                offerToAdd.Title,
                offerToAdd.Description,
                offerToAdd.InitialPrice,
                offerToAdd.ExpirationDateTime);
            var addedOffer = httpCreateOfferResponse.Content.ReadAsAsync<OfferModel>().Result;
            var url = string.Format("api/offers/details/{0}", addedOffer.Id);

            // Act
            var httpGetResponse = TestingEngine.HttpClient.GetAsync(url, CancellationToken.None).Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, httpGetResponse.StatusCode);
        }
    }
}
