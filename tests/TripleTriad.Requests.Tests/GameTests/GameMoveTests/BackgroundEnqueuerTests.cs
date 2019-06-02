using System.Threading.Tasks;
using Moq;
using TripleTriad.BackgroundTasks.Queue;
using TripleTriad.Requests.GameRequests;
using TripleTriad.Requests.Notifications;
using Xunit;

namespace TripleTriad.Requests.Tests.GameTests.GameMoveTests
{
    public class BackgroundEnqueuerTests
    {
        private const int GameId = 2;

        [Fact]
        public async Task Should_queue_room_notification()
        {
            var backgroundTaskQueue = new Mock<IBackgroundTaskQueue>();
            backgroundTaskQueue
                .Setup(x => x.QueueBackgroundTask(
                    It.IsAny<object>()));

            var subject = new GameMove.BackgroundEnqueuer(
                backgroundTaskQueue.Object);

            var request = new GameMove.Request();
            var response = new GameMove.Response
            {
                GameId = GameId
            };

            await subject.Process(request, response);

            backgroundTaskQueue.Verify(
                x => x.QueueBackgroundTask(
                    It.Is<RoomNotification>(y => y.GameId == GameId)));
        }
    }
}