using System;
using System.Threading.Tasks;
using Moq;
using TripleTriad.BackgroundTasks.Queue;
using TripleTriad.Requests.GameRequests;
using TripleTriad.Requests.Notifications;
using Xunit;

namespace TripleTriad.Requests.Tests.GameTests.GameCreateTests
{
    public class BackgroundEnqueuerTests
    {
        private const int GameId = 2;
        private static readonly Guid PlayerId = new Guid("9782407f-b28a-43cd-b5bc-c38873abea51");

        [Fact]
        public async Task Should_queue_lobby_notification()
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
                x => x.QueueBackgroundTask(It.IsAny<LobbyNotification>()));
        }
    }
}