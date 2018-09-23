using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using TripleTriad.Data.Entities;
using TripleTriad.Data.Enums;
using TripleTriad.Logic.Cards;
using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Extensions;
using TripleTriad.Logic.Steps;
using TripleTriad.Logic.Steps.Handlers;
using TripleTriad.Requests.GameRequests;
using TripleTriad.Requests.Exceptions;
using TripleTriad.Requests.Tests.Utils;
using Xunit;

namespace TripleTriad.Requests.Tests.GameTests.GameStartTests
{
    public class RequestHandlerTests
    {
        private static readonly Guid PlayerOneId = Guid.NewGuid();
        private static readonly Guid PlayerTwoId = Guid.NewGuid();

        private static readonly int GameId = 2;

        private static readonly IEnumerable<Card> Cards = new[]
        {
            AllCards.Squall,
            AllCards.Seifer,
            AllCards.Edea,
            AllCards.Rinoa,
            AllCards.Quistis
        };

        private Game CreateGame(GameData gameData = null)
        {
            gameData = gameData ?? new GameData
            {
                PlayerOneCards = Cards,
                PlayerTwoCards = Cards
            };

            return new Game()
            {
                GameId = GameId,
                PlayerOneId = PlayerOneId,
                PlayerTwoId = PlayerTwoId,
                Status = GameStatus.ChooseCards,
                Data = gameData.ToJson()
            };
        }

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

            var coinTossHandler = new Mock<IStepHandler<CoinTossStep>>();
            coinTossHandler
                .Setup(x => x.Run(It.IsAny<CoinTossStep>()))
                .Returns(new GameData
                {
                    PlayerOneWonCoinToss = coinTossIsHeads,
                    PlayerOneTurn = coinTossIsHeads
                });

            var createBoardHandler = new Mock<IStepHandler<CreateBoardStep>>();
            createBoardHandler
                .Setup(x => x.Run(It.IsAny<CreateBoardStep>()))
                .Returns<CreateBoardStep>(x => x.Data);

            var subject = new GameStart.RequestHandler(
                context,
                coinTossHandler.Object,
                createBoardHandler.Object);

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

            var coinTossHandler = new Mock<IStepHandler<CoinTossStep>>();

            var createBoardHandler = new Mock<IStepHandler<CreateBoardStep>>();

            var subject = new GameStart.RequestHandler(
                context,
                coinTossHandler.Object,
                createBoardHandler.Object);

            Func<Task> act = async () => await subject.Handle(command, default);

            act.Should()
                .Throw<GameNotFoundException>()
                .Where(e => e.GameId == GameId);
        }

        [Theory]
        [InlineData(GameStatus.Waiting)]
        [InlineData(GameStatus.InProgress)]
        [InlineData(GameStatus.Finished)]
        public async Task Should_throw_GameHasInvalidStatusException(GameStatus status)
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

            var coinTossHandler = new Mock<IStepHandler<CoinTossStep>>();

            var createBoardHandler = new Mock<IStepHandler<CreateBoardStep>>();

            var subject = new GameStart.RequestHandler(
                context,
                coinTossHandler.Object,
                createBoardHandler.Object);

            Func<Task> act = async () => await subject.Handle(command, default);

            act.Should()
                .Throw<GameHasInvalidStatusException>()
                .Where(x => x.GameId == GameId
                    && x.Status == status);
        }

        public static IEnumerable<object[]> PlayersStillSelecting = new[]
        {
            new object[]{null, null, true, true},
            new object[]{Cards, null, false, true},
            new object[]{null, Cards, true, false}
        };

        [Theory]
        [MemberData(nameof(PlayersStillSelecting))]
        public async Task Should_throw_PlayerStillSelectingCardsException(
            IEnumerable<Card> playerOneCards,
            IEnumerable<Card> playerTwoCards,
            bool playerOneStillSelecting,
            bool playerTwoStillSelecting)
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = this.CreateGame(new GameData
            {
                PlayerOneCards = playerOneCards,
                PlayerTwoCards = playerTwoCards
            });
            game.Status = GameStatus.ChooseCards;

            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var command = new GameStart.Request()
            {
                GameId = game.GameId
            };

            var coinTossHandler = new Mock<IStepHandler<CoinTossStep>>();

            var createBoardHandler = new Mock<IStepHandler<CreateBoardStep>>();

            var subject = new GameStart.RequestHandler(
                context,
                coinTossHandler.Object,
                createBoardHandler.Object);

            Func<Task> act = async () => await subject.Handle(command, default);

            act.Should()
                .Throw<PlayerStillSelectingCardsException>()
                .Where(e => e.GameId == GameId
                    && e.PlayerOne == playerOneStillSelecting
                    && e.PlayerTwo == playerTwoStillSelecting);
        }
    }
}