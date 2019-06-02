using System;
using System.Threading.Tasks;
using Moq;
using TripleTriad.BackgroundTasks.Queue;
using TripleTriad.Requests.GameRequests;
using TripleTriad.Requests.Notifications;
using Xunit;

namespace TripleTriad.Requests.Tests.GameTests.CardSelectTests
{
    public class BackgroundEnqueuerTests
    {
        private const int GameId = 2;
        private static readonly Guid PlayerId = new Guid("15577833-c271-409d-abf7-8f14083a17fa");

        [Fact]
        public async Task Should_queue_game_start_notification()
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
                GameId = GameId
            };

            response.SetQueueTask(true);

            await subject.Process(request, response);

            backgroundTaskQueue.Verify(
                x => x.QueueBackgroundTask(
                    It.Is<GameStartNotification>(
                        y => y.GameId == GameId)));
        }

        [Fact]
        public async Task Should_not_queue_game_start_notification()
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
                GameId = GameId
            };

            response.SetQueueTask(false);

            await subject.Process(request, response);

            backgroundTaskQueue.Verify(
                x => x.QueueBackgroundTask(It.IsAny<object>()),
                Times.Never);
        }
    }
}