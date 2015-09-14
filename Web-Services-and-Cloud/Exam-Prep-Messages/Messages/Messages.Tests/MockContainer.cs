namespace Messages.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Messages.Data.Models;
    using Moq;
    using News.Data.Repositories;

    public class MockContainer
    {
        public MockContainer()
        {
            this.ChannelRepositoryMock = new Mock<IRepository<Channel>>();
        }

        public Mock<IRepository<Channel>> ChannelRepositoryMock { get; set; }

        public void PrepareMocks()
        {
            this.SetupFakeChannels();
        }

        private void SetupFakeChannels()
        {
            var fakeChannels = new List<Channel>
            {
                new Channel {Id = 1, Name = "Bulgaria"},
                new Channel {Id = 2, Name = "Germany"},
                new Channel {Id = 3, Name = "Rome"},
                new Channel {Id = 4, Name = "CNN"}
            };

            this.ChannelRepositoryMock.Setup(r => r.All()).Returns(fakeChannels.AsQueryable());
            this.ChannelRepositoryMock.Setup(r => r.Find(It.IsAny<int>()))
                .Returns((int id) => { return fakeChannels.FirstOrDefault(c => c.Id == id); });
        }
    }
}