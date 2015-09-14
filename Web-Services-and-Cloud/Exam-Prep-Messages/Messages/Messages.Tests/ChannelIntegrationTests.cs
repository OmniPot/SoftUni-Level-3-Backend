namespace Messages.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using Messages.Tests.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ChannelIntegrationTests
    {
        [TestMethod]
        public void DeleteChannel_NonExistingChannel_ShouldReturn404NotFound()
        {
            // Arrange
            TestingEngine.CleanDatabase();

            var notExistingChannelId = 0;

            // Act
            var deleteChannelHttpResponse = TestingEngine.DeleteChannelHttpDelete(notExistingChannelId);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, deleteChannelHttpResponse.StatusCode);
        }

        [TestMethod]
        public void DeleteChannel_NonEmptyChannelMessages_ShouldReturn409Conflict()
        {
            // Arrange
            TestingEngine.CleanDatabase();

            var channelName = "channel" + DateTime.Now.Ticks;
            var httpPostResponse = TestingEngine.CreateChannelHttpPost(channelName);
            var channelModel = httpPostResponse.Content.ReadAsAsync<ChannelModel>().Result;

            var message = "Message";
            TestingEngine.SendChannelMessageHttpPost(channelModel.Name, message);

            // Act
            var deleteChannelHttpResponse = TestingEngine.DeleteChannelHttpDelete(channelModel.Id);

            // Assert
            Assert.AreEqual(HttpStatusCode.Conflict, deleteChannelHttpResponse.StatusCode);
            var httpGetResponse = TestingEngine.HttpClient.GetAsync("/api/channels").Result;
            var channels = httpGetResponse.Content.ReadAsAsync<List<ChannelModel>>().Result;
            Assert.IsTrue(channels.Any(c => c.Id.Equals(channelModel.Id)));
        }

        [TestMethod]
        public void DeleteChannel_ExistingChannel_ShouldReturn200Ok_DeleteMessage()
        {
            // Arrange
            TestingEngine.CleanDatabase();

            var channelName = "channel" + DateTime.Now.Ticks;
            var createdChannel = TestingEngine.CreateChannelHttpPost(channelName)
                .Content.ReadAsAsync<ChannelModel>().Result;

            // Act
            var deleteChannelHttpResponse = TestingEngine.DeleteChannelHttpDelete(createdChannel.Id);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, deleteChannelHttpResponse.StatusCode);
            var httpGetResponse = TestingEngine.HttpClient.GetAsync("/api/channels").Result;
            var channels = httpGetResponse.Content.ReadAsAsync<List<ChannelModel>>().Result;
            Assert.AreEqual(0, channels.Count);
            Assert.IsFalse(channels.Any(c => c.Id.Equals(createdChannel.Id)));
        }
    }
}