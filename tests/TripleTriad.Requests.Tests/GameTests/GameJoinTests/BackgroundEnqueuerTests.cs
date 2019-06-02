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
        private static readonly Guid HostId = new Guid("190bae35-7454-42be-ba66-818ea5a036bc");
        private static readonly Guid ChallengerId = new Guid("9b16e7b3-1287-4303-ac07-d87d36d1ed74");

        private readonly GameJoin.BackgroundEnqueuer subject;
        private readonly Mock<IBackgroundTaskQueue> backgroundTaskQueue = new Mock<IBackgroundTaskQueue>();

        public BackgroundEnqueuerTests()
        {
            this.backgroundTaskQueue
                .Setup(x => x.QueueBackgroundTask(
                    It.IsAny<object>()));

            this.subject = new GameJoin.BackgroundEnqueuer(
                this.backgroundTaskQueue.Object);
        }

        [Fact]
        public async Task Should_queue_host_user_notification()
        {
            var request = new GameJoin.Request
            {
                GameId = GameId,
                PlayerId = ChallengerId
            };

            var response = new GameJoin.Response
            {
                GameId = GameId,
                HostId = HostId
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
            var request = new GameJoin.Request
            {
                GameId = GameId,
                PlayerId = ChallengerId
            };

            var response = new GameJoin.Response
            {
                GameId = GameId,
                HostId = HostId
            };

            await this.subject.Process(request, response);

            this.backgroundTaskQueue.Verify(
                x => x.QueueBackgroundTask(
                    It.Is<UserNotification>(
                        y => y.GameId == GameId
                            && y.UserId == ChallengerId)));
        }

        [Fact]
        public async Task Should_queue_lobby_notification()
        {
            var request = new GameJoin.Request
            {
                GameId = GameId,
                PlayerId = ChallengerId
            };

            var response = new GameJoin.Response
            {
                GameId = GameId,
                HostId = HostId
            };

            await this.subject.Process(request, response);

            this.backgroundTaskQueue.Verify(
                x => x.QueueBackgroundTask(It.IsAny<LobbyNotification>()));
        }
    }
}