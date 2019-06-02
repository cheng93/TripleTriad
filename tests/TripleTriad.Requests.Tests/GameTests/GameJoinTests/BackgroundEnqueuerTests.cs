using System;
using System.Threading.Tasks;
using Moq;
using TripleTriad.BackgroundTasks.Queue;
using TripleTriad.Requests.GameRequests;
using TripleTriad.Requests.Notifications;
using Xunit;

namespace TripleTriad.Requests.Tests.GameTests.GameJoinTests
{
    public class BackgroundEnqueuerTests
    {
        private const int GameId = 2;
        private static readonly Guid PlayerId = new Guid("9b16e7b3-1287-4303-ac07-d87d36d1ed74");

        [Fact]
        public async Task Should_queue_room_notification()
        {
            var backgroundTaskQueue = new Mock<IBackgroundTaskQueue>();
            backgroundTaskQueue
                .Setup(x => x.QueueBackgroundTask(
                    It.IsAny<object>()));

            var subject = new GameJoin.BackgroundEnqueuer(
                backgroundTaskQueue.Object);

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

            backgroundTaskQueue.Verify(
                x => x.QueueBackgroundTask(
                    It.Is<RoomNotification>(y => y.GameId == GameId)));
        }

        [Fact]
        public async Task Should_queue_lobby_notification()
        {
            var backgroundTaskQueue = new Mock<IBackgroundTaskQueue>();
            backgroundTaskQueue
                .Setup(x => x.QueueBackgroundTask(
                    It.IsAny<object>()));

            var subject = new GameJoin.BackgroundEnqueuer(
                backgroundTaskQueue.Object);

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

            backgroundTaskQueue.Verify(
                x => x.QueueBackgroundTask(It.IsAny<LobbyNotification>()));
        }
    }
}