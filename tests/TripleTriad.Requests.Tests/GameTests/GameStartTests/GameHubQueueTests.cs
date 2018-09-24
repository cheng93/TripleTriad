using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Moq;
using TripleTriad.BackgroundTasks.Queue;
using TripleTriad.Data.Entities;
using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Extensions;
using TripleTriad.Requests.GameRequests;
using TripleTriad.Requests.Tests.Utils;
using TripleTriad.SignalR;
using Xunit;

namespace TripleTriad.Requests.Tests.GameTests.GameStartTests
{
    public class GameHubQueueTests
    {
        private static readonly int GameId = 2;

        private static GameData GameData = new GameData
        {
            Log = new[] { "Game has started " }
        };

        private static Game Game = new Game
        {
            GameId = GameId,
            Data = GameData.ToJson()
        };

        private static Mock<IBackgroundTaskQueue> CreateQueue()
        {
            var backgroundTaskQueue = new Mock<IBackgroundTaskQueue>();
            backgroundTaskQueue
                .Setup(x => x.QueueBackgroundTask(
                    It.IsAny<Func<CancellationToken, Task>>()))
                .Callback<Func<CancellationToken, Task>>(async x =>
                {
                    await x(default);
                });

            return backgroundTaskQueue;
        }

        private static Mock<IGameClient> CreateGameClient()
        {
            var gameClient = new Mock<IGameClient>();
            gameClient
                .Setup(x => x.Send(
                    It.IsAny<GameDataMessage>(),
                    It.IsAny<CancellationToken>()))
                .Verifiable();

            return gameClient;
        }

        private static Mock<IHubClients<IGameClient>> CreateHubClients(IGameClient gameClient)
        {
            var hubClients = new Mock<IHubClients<IGameClient>>();
            hubClients
                .Setup(x => x.Group(It.IsAny<string>()))
                .Returns(gameClient);

            return hubClients;
        }

        private static Mock<IHubContext<GameHub, IGameClient>> CreateHubContext(IHubClients<IGameClient> hubClients)
        {
            var hubContext = new Mock<IHubContext<GameHub, IGameClient>>();
            hubContext
                .Setup(x => x.Clients)
                .Returns(hubClients);

            return hubContext;
        }

        [Fact]
        public async Task Should_sent_message_to_hub()
        {
            var context = DbContextFactory.CreateTripleTriadContext();

            await context.Games.AddAsync(Game);
            await context.SaveChangesAsync();

            var request = new GameStart.Request();
            var response = new GameStart.Response
            {
                GameId = GameId
            };

            var backgroundTaskQueue = CreateQueue();
            var gameClient = CreateGameClient();
            var hubClients = CreateHubClients(gameClient.Object);
            var hubContext = CreateHubContext(hubClients.Object);

            var subject = new GameStart.GameHubQueue(
                backgroundTaskQueue.Object,
                context,
                hubContext.Object);

            await subject.Process(request, response);

            gameClient.Verify(x => x.Send(
                It.Is<GameDataMessage>(
                    y => Enumerable.SequenceEqual(y.Log, GameData.Log)),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task Should_select_correct_hub_group()
        {
            var context = DbContextFactory.CreateTripleTriadContext();

            await context.Games.AddAsync(Game);
            await context.SaveChangesAsync();

            var request = new GameStart.Request();
            var response = new GameStart.Response
            {
                GameId = GameId
            };

            var backgroundTaskQueue = CreateQueue();
            var gameClient = CreateGameClient();
            var hubClients = CreateHubClients(gameClient.Object);
            var hubContext = CreateHubContext(hubClients.Object);

            var subject = new GameStart.GameHubQueue(
                backgroundTaskQueue.Object,
                context,
                hubContext.Object);

            await subject.Process(request, response);

            hubClients.Verify(x => x.Group(
                It.Is<string>(y => y == GameId.ToString())));
        }
    }
}