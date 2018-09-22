using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using TripleTriad.Data.Entities;
using TripleTriad.Data.Enums;
using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Steps;
using TripleTriad.Logic.Steps.Strategies;
using TripleTriad.Requests.GameRequests;
using TripleTriad.Requests.GameRequests.Exceptions;
using TripleTriad.Requests.Tests.Utils;
using Xunit;

namespace TripleTriad.Requests.Tests.GameTests.GameStartTests
{
    public class RequestHandlerTests
    {
        private static readonly Guid PlayerOneId = Guid.NewGuid();
        private static readonly Guid PlayerTwoId = Guid.NewGuid();

        private static readonly int GameId = 2;

        private Game CreateGame() => new Game()
        {
            GameId = GameId,
            PlayerOneId = PlayerOneId,
            PlayerTwoId = PlayerTwoId,
            Data = JsonConvert.SerializeObject(new GameData())
        };

        private Player CreatePlayer(Guid playerId, bool isPlayerOne) => new Player()
        {
            PlayerId = playerId,
            DisplayName = $"Guest{(isPlayerOne ? "1" : "2")}"
        };

        public static IEnumerable<object[]> ExpectedResponse = new[]
        {
            new object[] { true, PlayerOneId },
            new object[] { false, PlayerTwoId }
        };

        [Theory]
        [MemberData(nameof(ExpectedResponse))]
        public async Task Should_return_correct_starting_player_id(bool coinTossIsHeads, Guid expectedStartingPlayerId)
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = this.CreateGame();
            var playerOne = this.CreatePlayer(PlayerOneId, true);
            var playerTwo = this.CreatePlayer(PlayerTwoId, false);

            await context.Players.AddAsync(playerOne);
            await context.Players.AddAsync(playerTwo);
            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var command = new GameStart.Request()
            {
                GameId = game.GameId
            };

            var coinTossStep = new Mock<IStepStrategy<CoinTossStep>>();
            coinTossStep
                .Setup(x => x.Run(It.IsAny<CoinTossStep>()))
                .Returns(new GameData
                {
                    PlayerOneWonCoinToss = coinTossIsHeads,
                    PlayerOneTurn = coinTossIsHeads
                });

            var subject = new GameStart.RequestHandler(context, coinTossStep.Object);

            var response = await subject.Handle(command, default);

            response.StartPlayerId.Should().Be(expectedStartingPlayerId);
        }

        [Fact]
        public void Should_throw_GameNotFoundException()
        {
            var context = DbContextFactory.CreateTripleTriadContext();

            var command = new GameStart.Request()
            {
                GameId = GameId
            };

            var coinTossStep = new Mock<IStepStrategy<CoinTossStep>>();

            var subject = new GameStart.RequestHandler(context, coinTossStep.Object);

            Func<Task> act = async () => await subject.Handle(command, default);

            act.Should().Throw<GameNotFoundException>();
        }

        [Theory]
        [InlineData(GameStatus.InProgress)]
        [InlineData(GameStatus.Forfeit)]
        [InlineData(GameStatus.PlayerOneWin)]
        [InlineData(GameStatus.PlayerTwoWin)]
        [InlineData(GameStatus.Tie)]
        public async Task Should_throw_GameHasStartedException(GameStatus status)
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = this.CreateGame();
            game.Status = status;

            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var command = new GameStart.Request()
            {
                GameId = game.GameId
            };

            var coinTossStep = new Mock<IStepStrategy<CoinTossStep>>();

            var subject = new GameStart.RequestHandler(context, coinTossStep.Object);

            Func<Task> act = async () => await subject.Handle(command, default);

            act.Should().Throw<GameHasStartedException>();
        }
    }
}