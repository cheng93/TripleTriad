using System;
using System.Threading.Tasks;
using Moq;
using TripleTriad.BackgroundTasks.Queue;
using TripleTriad.Requests.GameRequests;
using Xunit;

namespace TripleTriad.Requests.Tests.GameTests.GameCreateTests
{
    public class BackgroundEnqueuerTests
    {
        private static readonly int GameId = 2;
        private static readonly Guid PlayerId = Guid.NewGuid();

        [Fact]
        public async Task Should_queue_notification()
        {
            var backgroundTaskQueue = new Mock<IBackgroundTaskQueue>();
            backgroundTaskQueue
                .Setup(x => x.QueueBackgroundTask(
                    It.IsAny<object>()));

            var subject = new GameCreate.BackgroundEnqueuer(
                backgroundTaskQueue.Object);

            var request = new GameCreate.Request
            {
                PlayerId = PlayerId
            };

            var response = new GameCreate.Response
            {
                GameId = GameId
            };

            await subject.Process(request, response);

            backgroundTaskQueue.Verify(
                x => x.QueueBackgroundTask(response));
        }
    }
}