using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using TripleTriad.BackgroundTasks.Queue;
using TripleTriad.Requests.GameRequests;
using Xunit;

namespace TripleTriad.Requests.Tests.GameTests.GameJoinTests
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

            var subject = new GameJoin.GameStartBackgroundQueue(
                backgroundTaskQueue.Object,
                mediator.Object);

            var request = new GameJoin.Request
            {
                GameId = GameId,
                PlayerId = PlayerId
            };

            var response = new GameJoin.Response
            {
                GameId = GameId
            };

            await subject.Process(request, response);

            mediator.Verify(x => x.Send(
                It.Is<GameStart.Request>(y => y.GameId == GameId),
                It.IsAny<CancellationToken>()));
        }
    }
}