using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using TripleTriad.Requests.GameRequests;
using TripleTriad.Requests.HubRequests;
using TripleTriad.Requests.Messages;
using TripleTriad.SignalR;
using Xunit;

namespace TripleTriad.Requests.Tests.GameTests.GameCreateTests
{
    public class LobbyNotifierTests
    {
        private static readonly int GameId = 2;

        private static readonly string Message = "Hello World";

        [Fact]
        public async Task Should_send_request()
        {
            var mediator = new Mock<IMediator>();
            mediator
                .Setup(x => x.Send(
                    It.IsAny<HubGroupNotify.Request>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Unit());

            var messageFactory = new Mock<IMessageFactory<Messages.GameList.MessageData>>();
            messageFactory
                .Setup(x => x.Create(
                    It.IsAny<Messages.GameList.MessageData>()))
                .ReturnsAsync(Message);

            var subject = new GameCreate.LobbyNotifier(
                mediator.Object,
                messageFactory.Object);

            var response = new GameCreate.Response
            {
                GameId = GameId
            };

            await subject.Handle(response, default);

            mediator.Verify(x => x.Send(
                It.Is<HubGroupNotify.Request>(
                    y => y.Group == GameHub.Lobby
                        && y.Message == Message),
                It.IsAny<CancellationToken>()));
        }

    }
}