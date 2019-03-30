using System;
using System.Threading.Tasks;
using Moq;
using TripleTriad.BackgroundTasks.Queue;
using TripleTriad.Requests.GameRequests;
using Xunit;

namespace TripleTriad.Requests.Tests.GameTests.CardSelectTests
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

            var subject = new CardSelect.BackgroundEnqueuer(
                backgroundTaskQueue.Object);

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

            backgroundTaskQueue.Verify(
                x => x.QueueBackgroundTask(response));
        }

        [Fact]
        public async Task Should_not_queue_notification()
        {
            var backgroundTaskQueue = new Mock<IBackgroundTaskQueue>();
            backgroundTaskQueue
                .Setup(x => x.QueueBackgroundTask(
                    It.IsAny<object>()));

            var subject = new CardSelect.BackgroundEnqueuer(
                backgroundTaskQueue.Object);

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
                x => x.QueueBackgroundTask(response),
                Times.Never);
        }
    }
}