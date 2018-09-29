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
    public class GameStartBackgroundQueueTests
    {
        private static readonly int GameId = 2;
        private static readonly Guid PlayerId = Guid.NewGuid();

        [Fact]
        public async Task Should_queue_request()
        {
            var mediator = new Mock<IMediator>();
            mediator
                .Setup(x => x.Send(
                    It.IsAny<GameStart.Request>(),
                    It.IsAny<CancellationToken>()))
                .Verifiable();

            var backgroundTaskQueue = new Mock<IBackgroundTaskQueue>();
            backgroundTaskQueue
                .Setup(x => x.QueueBackgroundTask(
                    It.IsAny<Func<CancellationToken, Task>>()))
                .Callback<Func<CancellationToken, Task>>(async x =>
                {
                    await x(default);
                });

            var subject = new CardSelect.GameStartPostProcessor(
                backgroundTaskQueue.Object,
                mediator.Object);

            var request = new CardSelect.Request
            {
                GameId = GameId,
                PlayerId = PlayerId
            };

            var response = new CardSelect.Response
            {
                GameId = GameId,
                QueueTask = true
            };

            await subject.Process(request, response);

            mediator.Verify(x => x.Send(
                It.Is<GameStart.Request>(y => y.GameId == GameId),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task Should_not_queue_request()
        {
            var mediator = new Mock<IMediator>();

            var backgroundTaskQueue = new Mock<IBackgroundTaskQueue>();
            backgroundTaskQueue
                .Setup(x => x.QueueBackgroundTask(
                    It.IsAny<Func<CancellationToken, Task>>()))
                .Verifiable();

            var subject = new CardSelect.GameStartPostProcessor(
                backgroundTaskQueue.Object,
                mediator.Object);

            var request = new CardSelect.Request
            {
                GameId = GameId,
                PlayerId = PlayerId
            };

            var response = new CardSelect.Response
            {
                GameId = GameId,
                QueueTask = false
            };

            await subject.Process(request, response);

            backgroundTaskQueue.Verify(
                x => x.QueueBackgroundTask(
                    It.IsAny<Func<CancellationToken, Task>>()),
                Times.Never);
        }
    }
}