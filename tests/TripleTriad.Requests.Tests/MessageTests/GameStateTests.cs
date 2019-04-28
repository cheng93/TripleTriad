using System;
using System.Threading.Tasks;
using FluentAssertions.Json;
using Moq;
using Newtonsoft.Json.Linq;
using TripleTriad.Data.Entities;
using TripleTriad.Requests.Messages;
using TripleTriad.Requests.Messages.GameStateDataStrategies;
using TripleTriad.Requests.Tests.Utils;
using Xunit;

namespace TripleTriad.Requests.Tests.MessageTests
{
    public class GameStateTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("0d25184d-7dcb-4edf-bad6-ce415b46d21d")]
        public async Task Should_return_game_state_data(string playerId)
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = new Game() { GameId = 2 };

            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var dataStrategy = new Mock<IGameStateDataStrategy>();
            dataStrategy
                .Setup(x => x.GetData(It.IsAny<Game>(), It.IsAny<Guid?>()))
                .Returns(new GameStateData
                {
                    GameId = game.GameId,
                    Status = "Hello World"
                });

            var dataStrategyFactory = new Mock<IGameStateDataStrategyFactory>();
            dataStrategyFactory
                .Setup(x => x.GetStrategy(It.IsAny<Game>()))
                .Returns(dataStrategy.Object);

            var subject = new GameState.MessageFactory(
                context,
                dataStrategyFactory.Object);

            var message = await subject.Create(
                new GameState.MessageData
                {
                    GameId = game.GameId,
                    PlayerId = Guid.TryParse(playerId, out var g)
                        ? (Guid?)g
                        : null
                });

            var expected = "{ \"gameId\": 2, \"status\": \"Hello World\" }";

            JToken.Parse(message)["data"].Should().BeEquivalentTo(JToken.Parse(expected));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("0d25184d-7dcb-4edf-bad6-ce415b46d21d")]
        public async Task Should_return_correct_type(string playerId)
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = new Game() { GameId = 2 };

            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var dataStrategy = new Mock<IGameStateDataStrategy>();
            dataStrategy
                .Setup(x => x.GetData(It.IsAny<Game>(), It.IsAny<Guid?>()))
                .Returns(new GameStateData
                {
                    GameId = game.GameId,
                    Status = "Hello World"
                });

            var dataStrategyFactory = new Mock<IGameStateDataStrategyFactory>();
            dataStrategyFactory
                .Setup(x => x.GetStrategy(It.IsAny<Game>()))
                .Returns(dataStrategy.Object);

            var subject = new GameState.MessageFactory(
                context,
                dataStrategyFactory.Object);

            var message = await subject.Create(
                new GameState.MessageData
                {
                    GameId = game.GameId,
                    PlayerId = Guid.TryParse(playerId, out var g)
                        ? (Guid?)g
                        : null
                });

            var expected = "GameState";

            JToken.Parse(message)["type"].Should().HaveValue(expected);
        }
    }
}