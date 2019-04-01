using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using TripleTriad.Requests.GameRequests;
using TripleTriad.Requests.HubRequests;
using Xunit;

namespace TripleTriad.Requests.Tests.GameTests.GameJoinTests
{
    public class GameHubGroupNotifyNotificationHandlerTests
    {
        private static readonly int GameId = 2;
        private static readonly Guid PlayerId = Guid.NewGuid();

        [Fact]
        public async Task Should_send_request()
        {
            var mediator = new Mock<IMediator>();
            mediator
                .Setup(x => x.Send(
                    It.IsAny<GameHubGroupNotify.Request>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Unit());

            var subject = new GameJoin.GameHubGroupNotifyNotificationHandler(
                mediator.Object);

            var response = new GameJoin.Response
            {
                GameId = GameId
            };

            await subject.Handle(response, default);

            mediator.Verify(x => x.Send(
                It.Is<GameHubGroupNotify.Request>(y => y.GameId == GameId),
                It.IsAny<CancellationToken>()));
        }
    }
}