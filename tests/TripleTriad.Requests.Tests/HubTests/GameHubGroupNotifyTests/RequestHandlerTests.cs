using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Newtonsoft.Json;
using TripleTriad.Data.Entities;
using TripleTriad.Data.Enums;
using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Enums;
using TripleTriad.Logic.Extensions;
using TripleTriad.Requests.HubRequests;
using TripleTriad.Requests.Tests.Utils;
using TripleTriad.SignalR;
using Xunit;

namespace TripleTriad.Requests.Tests.HubTests.GameHubGroupNotifyTests
{
    public class RequestHandlerTests
    {
        private static readonly int GameId = 2;

        private static GameData CreateGameData()
            => new GameData
            {
                HostTurn = true,
                HostWonCoinToss = true,
                Result = Result.PlayerTwoWin
            };

        private static Game CreateGame(GameStatus gameStatus)
            => new Game
            {
                GameId = GameId,
                Status = gameStatus,
                Data = CreateGameData().ToJson()
            };

        public static IEnumerable<object[]> TestData = new[]
        {
            new object[]
            {
                GameStatus.Waiting,
                new
                {
                    type = "UpdateGame",
                    data = new
                    {
                        gameId = GameId,
                        status = GameStatus.Waiting.ToString()
                    }
                }
            },
            new object[]
            {
                GameStatus.ChooseCards,
                new
                {
                    type = "UpdateGame",
                    data = new
                    {
                        gameId = GameId,
                        status = GameStatus.ChooseCards.ToString()
                    }
                }
            },
            new object[]
            {
                GameStatus.InProgress,
                new
                {
                    type = "UpdateGame",
                    data = new
                    {
                        gameId = GameId,
                        status = GameStatus.InProgress.ToString(),
                        log = new string[] { },
                        hostTurn = true,
                        hostWonCoinToss = true,
                        tiles = (IEnumerable<Tile>)null
                    }
                }
            },
            new object[]
            {
                GameStatus.Finished,
                new
                {
                    type = "UpdateGame",
                    data = new
                    {
                        gameId = GameId,
                        status = GameStatus.Finished.ToString(),
                        log = new string[] { },
                        hostTurn = true,
                        hostWonCoinToss = true,
                        tiles = (IEnumerable<Tile>)null,
                        result = "PlayerTwoWin"
                    }
                }
            }
        };

        [Theory]
        [MemberData(nameof(TestData))]
        public async Task Should_send_message(GameStatus status, object message)
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = CreateGame(status);

            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var request = new GameHubGroupNotify.Request()
            {
                GameId = GameId
            };

            var gameClient = new Mock<IGameClient>();
            gameClient
                .Setup(x => x.Send(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var hubClients = new Mock<IHubClients<IGameClient>>();
            hubClients
                .Setup(x => x.Group(It.IsAny<string>()))
                .Returns(gameClient.Object);

            var hubContext = new Mock<IHubContext<GameHub, IGameClient>>();
            hubContext
                .Setup(x => x.Clients)
                .Returns(hubClients.Object);

            var subject = new GameHubGroupNotify.RequestHandler(
                context,
                hubContext.Object);

            await subject.Handle(request, default);

            var serializedMessage = JsonConvert.SerializeObject(message);

            gameClient.Verify(x => x.Send(
                It.Is<string>(y => y == serializedMessage)));
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public async Task Should_choose_correct_group(GameStatus status, object message)
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = CreateGame(status);

            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var request = new GameHubGroupNotify.Request()
            {
                GameId = GameId
            };

            var gameClient = new Mock<IGameClient>();
            gameClient
                .Setup(x => x.Send(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var hubClients = new Mock<IHubClients<IGameClient>>();
            hubClients
                .Setup(x => x.Group(It.IsAny<string>()))
                .Returns(gameClient.Object);

            var hubContext = new Mock<IHubContext<GameHub, IGameClient>>();
            hubContext
                .Setup(x => x.Clients)
                .Returns(hubClients.Object);

            var subject = new GameHubGroupNotify.RequestHandler(
                context,
                hubContext.Object);

            await subject.Handle(request, default);

            var serializedMessage = JsonConvert.SerializeObject(message);

            hubClients.Verify(x => x.Group(
                It.Is<string>(y => y == GameId.ToString())));
        }
    }
}