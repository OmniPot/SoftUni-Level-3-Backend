namespace BidSystem.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BidSystem.Data.Contracts;
    using BidSystem.Data.Models;
    using Moq;

    public class MockContainer
    {
        public MockContainer()
        {
            this.UsersRepositoryMock = new Mock<IRepository<User>>();
            this.OffersRepositoryMock = new Mock<IRepository<Offer>>();
            this.BidsRepositoryMock = new Mock<IRepository<Bid>>();
        }

        public Mock<IRepository<User>> UsersRepositoryMock { get; set; }

        public Mock<IRepository<Offer>> OffersRepositoryMock { get; set; }

        public Mock<IRepository<Bid>> BidsRepositoryMock { get; set; }

        public void PrepareMocks()
        {
            this.SetupFakeUsers();
            this.SetupFakeOffers();
            this.SetupFakeBids();
        }

        private void SetupFakeUsers()
        {
            var fakeUsers = new List<User>
            {
                new User { UserName = "Iliq", Id = "123" },
                new User { UserName = "Stamen", Id = "456" }
            };

            this.UsersRepositoryMock.Setup(r => r.All()).Returns(fakeUsers.AsQueryable());
            this.UsersRepositoryMock.Setup(r => r.Find(It.IsAny<string>()))
                .Returns((string id) => { return fakeUsers.FirstOrDefault(u => u.Id == id); });
        }

        private void SetupFakeOffers()
        {
            var fakeOffers = new List<Offer>
            {
                new Offer{Id = 1,Title = "Title 1", Description = "Description 1", PublishData = DateTime.Now.AddDays(-10), ExpirationDateTime = DateTime.Now.AddDays(-3), InitialPrice = 100m, SellerId = "123"},
                new Offer{Id = 2,Title = "Title 2", Description = "Description 2", PublishData = DateTime.Now.AddDays(-11), ExpirationDateTime = DateTime.Now.AddDays(-1), InitialPrice = 200m, SellerId = "123"},
                new Offer{Id = 3,Title = "Title 3", Description = "Description 3", PublishData = DateTime.Now.AddDays(-12), ExpirationDateTime = DateTime.Now.AddDays(2), InitialPrice = 300m, SellerId = "456"},
                new Offer{Id = 4,Title = "Title 4", Description = "Description 4", PublishData = DateTime.Now.AddDays(-13), ExpirationDateTime = DateTime.Now.AddDays(4), InitialPrice = 400m, SellerId = "456"}
            };

            this.OffersRepositoryMock.Setup(r => r.All()).Returns(fakeOffers.AsQueryable());
            this.OffersRepositoryMock.Setup(r => r.Find(It.IsAny<string>()))
                .Returns((int id) => { return fakeOffers.FirstOrDefault(u => u.Id == id); });
        }

        private void SetupFakeBids()
        {
            var fakeBids = new List<Bid>
            {
                new Bid{ Id = 1,Price = 200m, BidDate = DateTime.Now.AddDays(-1), Comment = "Comment fake 1", OfferId = 1, BidderId = "456", Bidder = this.UsersRepositoryMock.Object.Find("456")},
                new Bid{ Id = 2,Price = 300m, BidDate = DateTime.Now.AddDays(-2), Comment = "Comment fake 2", OfferId = 2, BidderId = "456", Bidder = this.UsersRepositoryMock.Object.Find("456")},
                new Bid{ Id = 3,Price = 400m, BidDate = DateTime.Now.AddDays(-3), Comment = "Comment fake 3", OfferId = 2, BidderId = "123", Bidder = this.UsersRepositoryMock.Object.Find("123")},
                new Bid{ Id = 4,Price = 500m, BidDate = DateTime.Now.AddDays(-4), Comment = "Comment fake 4", OfferId = 4, BidderId = "123", Bidder = this.UsersRepositoryMock.Object.Find("123")}
            };

            this.BidsRepositoryMock.Setup(r => r.All()).Returns(fakeBids.AsQueryable());
            this.BidsRepositoryMock.Setup(r => r.Find(It.IsAny<int>()))
                .Returns((int id) => { return fakeBids.FirstOrDefault(c => c.Id == id); });
        }
    }
}