using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using TripleTriad.BackgroundTasks.Queue;
using TripleTriad.Data;
using TripleTriad.Data.Entities;
using TripleTriad.Requests.GameRequests;
using TripleTriad.Requests.HubRequests;
using TripleTriad.Requests.Messages;
using TripleTriad.Requests.Tests.Utils;
using TripleTriad.SignalR;
using Xunit;

namespace TripleTriad.Requests.Tests.GameTests.GameMoveTests
{
    public class RoomNotifierTests
    {
        private const int GameId = 2;
        private const string Message = "Hello World";
        private static readonly Guid HostId = new Guid("3adae823-0431-4a43-8c6b-d6ce72edc985");
        private const string HostConnectionId = "2c5a8695-8eb4-4308-81b3-43478b6febc2";
        private static readonly Guid ChallengerId = new Guid("478feaf3-9d3b-47c6-94bf-d070f32595bb");
        private const string ChallengerConnectionId = "0378bb2e-2696-4533-b955-eb5a3ce600ac";

        private readonly Mock<IMessageFactory<Messages.GameState.MessageData>> messageFactory = new Mock<IMessageFactory<Messages.GameState.MessageData>>();
        private readonly Mock<IMediator> mediator = new Mock<IMediator>();
        private readonly Mock<IConnectionIdStore> connectionIdStore = new Mock<IConnectionIdStore>();

        public RoomNotifierTests()
        {
            this.mediator
                .Setup(x => x.Send(
                    It.IsAny<HubGroupNotify.Request>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Unit());

            this.messageFactory
                .Setup(x => x.Create(
                    It.IsAny<Messages.GameState.MessageData>()))
                .ReturnsAsync(Message);

            this.connectionIdStore
                .Setup(x => x.GetConnectionIds(HostId.ToString()))
                .ReturnsAsync(new[] { HostConnectionId });
            this.connectionIdStore
                .Setup(x => x.GetConnectionIds(ChallengerId.ToString()))
                .ReturnsAsync(new[] { ChallengerConnectionId });
        }

        private static Game CreateGame()
            => new Game
            {
                GameId = GameId,
                HostId = HostId,
                ChallengerId = ChallengerId
            };

        private async Task<TripleTriadDbContext> SetupContext()
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = CreateGame();

            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            return context;
        }

        [Fact]
        public async Task Should_send_request()
        {
            var context = await this.SetupContext();
            var subject = new GameMove.RoomNotifier(
                context,
                this.mediator.Object,
                this.messageFactory.Object,
                this.connectionIdStore.Object);

            var response = new GameMove.Response
            {
                GameId = GameId
            };

            await subject.Handle(response, default);

            var playerIds = new[]
            {
                HostConnectionId,
                ChallengerConnectionId
            };

            this.mediator.Verify(x => x.Send(
                It.Is<HubGroupNotify.Request>(
                    y => y.Group == GameId.ToString()
                        && y.Message == Message
                        && new HashSet<string>(y.Excluded).SetEquals(playerIds)),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task Should_get_message_with_correct_parameters()
        {
            var context = await this.SetupContext();

            var subject = new GameMove.RoomNotifier(
                context,
                this.mediator.Object,
                this.messageFactory.Object,
                this.connectionIdStore.Object);

            var response = new GameMove.Response
            {
                GameId = GameId
            };

            await subject.Handle(response, default);

            this.messageFactory.Verify(x => x.Create(
                It.Is<Messages.GameState.MessageData>(
                    y => y.GameId == GameId
                        && !y.PlayerId.HasValue)));
        }
    }
}