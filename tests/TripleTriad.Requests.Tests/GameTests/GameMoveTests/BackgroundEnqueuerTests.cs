using System;
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
        private static readonly Guid HostId = new Guid("3fdc4bfe-f487-4555-8905-c6c9c89bbc21");
        private static readonly Guid ChallengerId = new Guid("ab796777-a8d9-41e1-8a3a-93d408d68192");

        private readonly GameMove.BackgroundEnqueuer subject;
        private readonly Mock<IBackgroundTaskQueue> backgroundTaskQueue = new Mock<IBackgroundTaskQueue>();

        public BackgroundEnqueuerTests()
        {
            this.backgroundTaskQueue
                .Setup(x => x.QueueBackgroundTask(
                    It.IsAny<object>()));

            this.subject = new GameMove.BackgroundEnqueuer(
                this.backgroundTaskQueue.Object);
        }

        [Fact]
        public async Task Should_queue_room_notification()
        {
            var request = new GameMove.Request();
            var response = new GameMove.Response
            {
                GameId = GameId
            };

            await this.subject.Process(request, response);

            this.backgroundTaskQueue.Verify(
                x => x.QueueBackgroundTask(
                    It.Is<RoomNotification>(y => y.GameId == GameId)));
        }

        [Fact]
        public async Task Should_queue_host_user_notification()
        {
            var request = new GameMove.Request();
            var response = new GameMove.Response
            {
                GameId = GameId,
                HostId = HostId,
                ChallengerId = ChallengerId
            };

            await this.subject.Process(request, response);

            this.backgroundTaskQueue.Verify(
                x => x.QueueBackgroundTask(
                    It.Is<UserNotification>(
                        y => y.GameId == GameId
                            && y.UserId == HostId)));
        }

        [Fact]
        public async Task Should_queue_challenger_user_notification()
        {
            var request = new GameMove.Request();
            var response = new GameMove.Response
            {
                GameId = GameId,
                HostId = HostId,
                ChallengerId = ChallengerId
            };

            await this.subject.Process(request, response);

            this.backgroundTaskQueue.Verify(
                x => x.QueueBackgroundTask(
                    It.Is<UserNotification>(
                        y => y.GameId == GameId
                            && y.UserId == ChallengerId)));
        }
    }
}