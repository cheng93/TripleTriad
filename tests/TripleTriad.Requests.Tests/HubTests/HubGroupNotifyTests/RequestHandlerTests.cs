using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Moq;
using TripleTriad.Requests.HubRequests;
using TripleTriad.SignalR;
using Xunit;

namespace TripleTriad.Requests.Tests.HubTests.HubGroupNotifyTests
{
    public class RequestHandlerTests
    {
        [Theory]
        [InlineData("Hello World")]
        [InlineData("FooBar")]
        public async Task Should_send_message(string message)
        {
            var request = new HubGroupNotify.Request()
            {
                Group = "1",
                Message = message
            };

            var gameClient = new Mock<IGameClient>();
            gameClient
                .Setup(x => x.Send(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var hubClients = new Mock<IHubClients<IGameClient>>();
            hubClients
                .Setup(x => x.GroupExcept(It.IsAny<string>(), It.IsAny<IReadOnlyList<string>>()))
                .Returns(gameClient.Object);

            var hubContext = new Mock<IHubContext<GameHub, IGameClient>>();
            hubContext
                .Setup(x => x.Clients)
                .Returns(hubClients.Object);

            var subject = new HubGroupNotify.RequestHandler(
                hubContext.Object);

            await subject.Handle(request, default);

            gameClient.Verify(x => x.Send(message));
        }


        [Theory]
        [InlineData("1")]
        [InlineData(GameHub.Lobby)]
        public async Task Should_choose_correct_group(string group)
        {
            var request = new HubGroupNotify.Request()
            {
                Group = group,
                Message = "Hello World"
            };

            var gameClient = new Mock<IGameClient>();
            gameClient
                .Setup(x => x.Send(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var hubClients = new Mock<IHubClients<IGameClient>>();
            hubClients
                .Setup(x => x.GroupExcept(It.IsAny<string>(), It.IsAny<IReadOnlyList<string>>()))
                .Returns(gameClient.Object);

            var hubContext = new Mock<IHubContext<GameHub, IGameClient>>();
            hubContext
                .Setup(x => x.Clients)
                .Returns(hubClients.Object);

            var subject = new HubGroupNotify.RequestHandler(
                hubContext.Object);

            await subject.Handle(request, default);

            hubClients.Verify(x => x.GroupExcept(
                group,
                It.Is<IReadOnlyList<string>>(y => y.Count == 0)));
        }

        [Theory]
        [InlineData("1")]
        [InlineData(GameHub.Lobby)]
        public async Task Should_choose_correct_group_and_exclude(string group)
        {
            var excludedIds = new[] { "a6eff1fb-8b8c-4f2d-b6f3-85f57a33cd60", "20255bec-d48d-4309-972f-4b74c8f06fc8" };

            var request = new HubGroupNotify.Request()
            {
                Group = group,
                Message = "Hello World",
                Excluded = excludedIds
            };

            var gameClient = new Mock<IGameClient>();
            gameClient
                .Setup(x => x.Send(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var hubClients = new Mock<IHubClients<IGameClient>>();
            hubClients
                .Setup(x => x.GroupExcept(It.IsAny<string>(), It.IsAny<IReadOnlyList<string>>()))
                .Returns(gameClient.Object);

            var hubContext = new Mock<IHubContext<GameHub, IGameClient>>();
            hubContext
                .Setup(x => x.Clients)
                .Returns(hubClients.Object);

            var subject = new HubGroupNotify.RequestHandler(
                hubContext.Object);

            await subject.Handle(request, default);

            hubClients.Verify(x => x.GroupExcept(
                group,
                It.Is<IReadOnlyList<string>>(y => new HashSet<string>(y).SetEquals(excludedIds))));
        }
    }
}