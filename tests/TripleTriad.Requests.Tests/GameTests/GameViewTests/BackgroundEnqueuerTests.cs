using System;
using System.Threading.Tasks;
using Moq;
using TripleTriad.BackgroundTasks.Queue;
using TripleTriad.Requests.GameRequests;
using Xunit;

namespace TripleTriad.Requests.Tests.GameTests.GameViewTests
{
    public class BackgroundEnqueuerTests
    {
        private static readonly int GameId = 2;
        private static readonly Guid PlayerId = Guid.NewGuid();

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public async Task Should_queue_notification(bool isHost, bool isChallenger)
        {
            var backgroundTaskQueue = new Mock<IBackgroundTaskQueue>();
            backgroundTaskQueue
                .Setup(x => x.QueueBackgroundTask(
                    It.IsAny<object>()));

            var subject = new GameView.BackgroundEnqueuer(
                backgroundTaskQueue.Object);

            var request = new GameView.Request
            {
                GameId = GameId,
                PlayerId = PlayerId
            };

            var response = new GameView.Response
            {
                GameId = GameId,
                IsHost = isHost,
                IsChallenger = isChallenger,
                PlayerId = PlayerId
            };

            await subject.Process(request, response);

            backgroundTaskQueue.Verify(
                x => x.QueueBackgroundTask(response));
        }
    }
}