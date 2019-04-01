using System.Threading.Tasks;
using Moq;
using TripleTriad.BackgroundTasks.Queue;
using TripleTriad.Requests.GameRequests;
using Xunit;

namespace TripleTriad.Requests.Tests.GameTests.GameStartTests
{
    public class BackgroundEnqueuerTests
    {
        private static readonly int GameId = 2;

        [Fact]
        public async Task Should_queue_notification()
        {
            var backgroundTaskQueue = new Mock<IBackgroundTaskQueue>();
            backgroundTaskQueue
                .Setup(x => x.QueueBackgroundTask(
                    It.IsAny<object>()));

            var subject = new GameStart.BackgroundEnqueuer(
                backgroundTaskQueue.Object);

            var request = new GameStart.Request();
            var response = new GameStart.Response
            {
                GameId = GameId
            };

            await subject.Process(request, response);

            backgroundTaskQueue.Verify(
                x => x.QueueBackgroundTask(response));
        }

    }
}