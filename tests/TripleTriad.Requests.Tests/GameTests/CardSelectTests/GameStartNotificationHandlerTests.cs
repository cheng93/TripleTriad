using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using TripleTriad.BackgroundTasks.Queue;
using TripleTriad.Requests.GameRequests;
using Xunit;

namespace TripleTriad.Requests.Tests.GameTests.CardSelectTests
{
    public class GameStartNotificationHandlerTests
    {
        private static readonly int GameId = 2;
        private static readonly Guid PlayerId = Guid.NewGuid();

        [Fact]
        public async Task Should_send_request()
        {
            var mediator = new Mock<IMediator>();
            mediator
                .Setup(x => x.Send(
                    It.IsAny<GameStart.Request>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GameStart.Response());

            var subject = new CardSelect.GameStartNotificationHandler(
                mediator.Object);

            var response = new CardSelect.Response
            {
                GameId = GameId,
                QueueTask = true
            };

            await subject.Handle(response, default);

            mediator.Verify(x => x.Send(
                It.Is<GameStart.Request>(y => y.GameId == GameId),
                It.IsAny<CancellationToken>()));
        }
    }
}