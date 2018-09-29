using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Newtonsoft.Json;
using TripleTriad.Data.Entities;
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
                PlayerOneTurn = true,
                PlayerOneWonCoinToss = true,
                Result = Result.PlayerTwoWin
            };

        private static Game CreateGame()
            => new Game
            {
                GameId = GameId,
                Data = CreateGameData().ToJson()
            };

        [Fact]
        public async Task Should_send_message()
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = CreateGame();

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

            var message = JsonConvert.SerializeObject(new
            {
                GameId = GameId,
                Log = new string[] { },
                PlayerOneTurn = true,
                PlayerOneWonCoinToss = true,
                Tiles = (IEnumerable<Tile>)null,
                Result = "PlayerTwoWin"
            });

            gameClient.Verify(x => x.Send(
                It.Is<string>(y => y == message)));
        }

        [Fact]
        public async Task Should_choose_correct_group()
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = CreateGame();

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

            var message = JsonConvert.SerializeObject(new
            {
                GameId = GameId,
                Log = new string[] { },
                PlayerOneTurn = true,
                PlayerOneWonCoinToss = true,
                Tiles = (IEnumerable<Tile>)null,
                Result = "PlayerTwoWin"
            });

            hubClients.Verify(x => x.Group(
                It.Is<string>(y => y == GameId.ToString())));
        }
    }
}