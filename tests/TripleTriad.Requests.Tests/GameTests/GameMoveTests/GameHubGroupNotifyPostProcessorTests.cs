using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using TripleTriad.BackgroundTasks.Queue;
using TripleTriad.Requests.GameRequests;
using TripleTriad.Requests.HubRequests;
using Xunit;

namespace TripleTriad.Requests.Tests.GameTests.GameMoveTests
{
    public class GameHubGroupNotifyPostProcessorTests
    {
        private static readonly int GameId = 2;

        private static Mock<IBackgroundTaskQueue> CreateQueue()
        {
            var backgroundTaskQueue = new Mock<IBackgroundTaskQueue>();
            backgroundTaskQueue
                .Setup(x => x.QueueBackgroundTask(
                    It.IsAny<Func<CancellationToken, Task>>()))
                .Callback<Func<CancellationToken, Task>>(async x =>
                {
                    await x(default);
                });

            return backgroundTaskQueue;
        }

        [Fact]
        public async Task Should_queue_request()
        {
            var mediator = new Mock<IMediator>();
            mediator
                .Setup(x => x.Send(
                    It.IsAny<GameHubGroupNotify.Request>(),
                    It.IsAny<CancellationToken>()))
                .Verifiable();

            var request = new GameMove.Request();
            var response = new GameMove.Response
            {
                GameId = GameId
            };

            var backgroundTaskQueue = CreateQueue();

            var subject = new GameMove.GameHubGroupNotifyPostProcessor(
                backgroundTaskQueue.Object,
                mediator.Object);

            await subject.Process(request, response);

            mediator.Verify(x => x.Send(
                It.Is<GameHubGroupNotify.Request>(y => y.GameId == GameId),
                It.IsAny<CancellationToken>()));
        }
    }
}