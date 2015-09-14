namespace Messages.Tests
{
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Web.Http;
    using Messages.Data;
    using Messages.RestServices.Controllers;
    using Messages.Tests.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ChannelUnitTestsWithMocking
    {
        private ChannelsController channelsController;
        private Mock<IMessagesData> mockContext;
        private MockContainer mocks;

        [TestInitialize]
        public void InitTests()
        {
            this.mocks = new MockContainer();

            this.mockContext = new Mock<IMessagesData>();
            this.mockContext.Setup(c => c.Channels).Returns(this.mocks.ChannelRepositoryMock.Object);

            this.channelsController = new ChannelsController(this.mockContext.Object);
            this.ConfigureController(this.channelsController);

            this.mocks.PrepareMocks();
        }

        [TestMethod]
        public void GetChannelById_ExistingId_ShouldReturn200Ok_ChannelModel()
        {
            var channel = this.mocks.ChannelRepositoryMock.Object.All().Last();

            var httpGetResponse =
                this.channelsController.GetChannelById(channel.Id).ExecuteAsync(CancellationToken.None).Result;
            var dbChannel = httpGetResponse.Content.ReadAsAsync<ChannelModel>().Result;

            Assert.AreEqual(HttpStatusCode.OK, httpGetResponse.StatusCode);
            Assert.AreEqual(channel.Id, dbChannel.Id);
            Assert.AreEqual(channel.Name, dbChannel.Name);
        }

        [TestMethod]
        public void GetChannelById_NonExistingId_ShouldReturn400NotFound()
        {
            var channelId = 0;

            var httpGetResponse = this.channelsController.GetChannelById(channelId)
                .ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.NotFound, httpGetResponse.StatusCode);
        }

        private void ConfigureController(ChannelsController controller)
        {
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
        }
    }
}