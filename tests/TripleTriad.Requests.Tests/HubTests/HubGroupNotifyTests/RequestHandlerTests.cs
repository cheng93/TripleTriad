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
                .Setup(x => x.Group(It.IsAny<string>()))
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
                .Setup(x => x.Group(It.IsAny<string>()))
                .Returns(gameClient.Object);

            var hubContext = new Mock<IHubContext<GameHub, IGameClient>>();
            hubContext
                .Setup(x => x.Clients)
                .Returns(hubClients.Object);

            var subject = new HubGroupNotify.RequestHandler(
                hubContext.Object);

            await subject.Handle(request, default);

            hubClients.Verify(x => x.Group(group));
        }
    }
}