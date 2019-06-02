using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using TripleTriad.Requests.GameRequests;
using TripleTriad.Requests.Notifications;
using Xunit;

namespace TripleTriad.Requests.Tests.NotificationTests
{
    public class GameStartNotificationHandlerTests
    {
        private const int GameId = 2;

        [Fact]
        public async Task Should_send_request()
        {
            var mediator = new Mock<IMediator>();
            mediator
                .Setup(x => x.Send(
                    It.IsAny<GameStart.Request>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GameStart.Response());

            var subject = new GameStartNotificationHandler(
                mediator.Object);

            var notification = new GameStartNotification(GameId);

            await subject.Handle(notification, default);

            mediator.Verify(x => x.Send(
                It.Is<GameStart.Request>(y => y.GameId == GameId),
                It.IsAny<CancellationToken>()));
        }
    }
}