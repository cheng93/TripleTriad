using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using TripleTriad.BackgroundTasks.Queue;
using TripleTriad.Requests.GameRequests;
using TripleTriad.Requests.HubRequests;
using Xunit;

namespace TripleTriad.Requests.Tests.GameTests.GameStartTests
{
    public class GameHubGroupNotifyNotificationHandlerTests
    {
        private static readonly int GameId = 2;

        [Fact]
        public async Task Should_send_request()
        {
            var mediator = new Mock<IMediator>();
            mediator
                .Setup(x => x.Send(
                    It.IsAny<GameHubGroupNotify.Request>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Unit());

            var response = new GameStart.Response
            {
                GameId = GameId
            };

            var subject = new GameStart.GameHubGroupNotifyNotificationHandler(
                mediator.Object);

            await subject.Handle(response, default);

            mediator.Verify(x => x.Send(
                It.Is<GameHubGroupNotify.Request>(y => y.GameId == GameId),
                It.IsAny<CancellationToken>()));
        }
    }
}