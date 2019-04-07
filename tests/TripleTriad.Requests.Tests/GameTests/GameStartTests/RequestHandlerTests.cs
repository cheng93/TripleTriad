using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using TripleTriad.Data.Entities;
using TripleTriad.Data.Enums;
using TripleTriad.Logic.Cards;
using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Exceptions;
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
        private static readonly Guid HostId = Guid.NewGuid();
        private static readonly Guid ChallengerId = Guid.NewGuid();

        private static readonly int GameId = 2;

        private static readonly IEnumerable<Card> Cards = new[]
        {
            AllCards.Squall,
            AllCards.Seifer,
            AllCards.Edea,
            AllCards.Rinoa,
            AllCards.Quistis
        };

        private static Game CreateGame(GameData gameData = null)
        {
            gameData = gameData ?? new GameData
            {
                HostCards = Cards,
                ChallengerCards = Cards
            };

            return new Game()
            {
                GameId = GameId,
                HostId = HostId,
                ChallengerId = ChallengerId,
                Status = GameStatus.ChooseCards,
                Data = gameData.ToJson()
            };
        }

        private static Player CreatePlayer(Guid playerId, bool isHost) => new Player()
        {
            PlayerId = playerId,
            DisplayName = $"Guest{(isHost ? "1" : "2")}"
        };

        public static IEnumerable<object[]> ExpectedResponse = new[]
        {
            new object[] { true, HostId },
            new object[] { false, ChallengerId }
        };

        [Theory]
        [MemberData(nameof(ExpectedResponse))]
        public async Task Should_return_correct_starting_player_id(bool coinTossIsHeads, Guid expectedStartingPlayerId)
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = CreateGame();
            var host = CreatePlayer(HostId, true);
            var challenger = CreatePlayer(ChallengerId, false);

            await context.Players.AddAsync(host);
            await context.Players.AddAsync(challenger);
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
                    HostWonCoinToss = coinTossIsHeads,
                    HostTurn = coinTossIsHeads
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
            var game = CreateGame();
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
            new object[]{true, true},
            new object[]{false, true},
            new object[]{true, false}
        };

        [Theory]
        [MemberData(nameof(PlayersStillSelecting))]
        public async Task Should_throw_inner_exception_PlayerStillSelectingCardsException(
            bool hostStillSelecting,
            bool challengerStillSelecting)
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = CreateGame();
            var host = CreatePlayer(HostId, true);
            var challenger = CreatePlayer(ChallengerId, false);

            await context.Players.AddAsync(host);
            await context.Players.AddAsync(challenger);
            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var command = new GameStart.Request()
            {
                GameId = game.GameId
            };

            var coinTossHandler = new Mock<IStepHandler<CoinTossStep>>();
            coinTossHandler
                .Setup(x => x.ValidateAndThrow(It.IsAny<CoinTossStep>()))
                .Throws(new PlayerStillSelectingCardsException(
                    new GameData(),
                    hostStillSelecting,
                    challengerStillSelecting));

            var createBoardHandler = new Mock<IStepHandler<CreateBoardStep>>();

            var subject = new GameStart.RequestHandler(
                context,
                coinTossHandler.Object,
                createBoardHandler.Object);

            Func<Task> act = async () => await subject.Handle(command, default);

            act.Should()
                .Throw<GameDataInvalidException>()
                .Where(e => e.GameId == GameId)
                .WithInnerException<PlayerStillSelectingCardsException>()
                .Where(e => e.Host == hostStillSelecting
                    && e.Challenger == challengerStillSelecting);
        }

        [Fact]
        public async Task Should_throw_inner_exception_BoardExistsException()
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = CreateGame();
            var host = CreatePlayer(HostId, true);
            var challenger = CreatePlayer(ChallengerId, false);

            await context.Players.AddAsync(host);
            await context.Players.AddAsync(challenger);
            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var command = new GameStart.Request()
            {
                GameId = game.GameId
            };

            var coinTossHandler = new Mock<IStepHandler<CoinTossStep>>();
            coinTossHandler
                .Setup(x => x.Run(It.IsAny<CoinTossStep>()))
                .Returns(new GameData());

            var createBoardHandler = new Mock<IStepHandler<CreateBoardStep>>();
            createBoardHandler
                .Setup(x => x.ValidateAndThrow(It.IsAny<CreateBoardStep>()))
                .Throws(new BoardExistsException(new GameData()));

            var subject = new GameStart.RequestHandler(
                context,
                coinTossHandler.Object,
                createBoardHandler.Object);

            Func<Task> act = async () => await subject.Handle(command, default);

            act.Should()
                .Throw<GameDataInvalidException>()
                .Where(e => e.GameId == GameId)
                .WithInnerException<BoardExistsException>();
        }

        [Fact]
        public async Task Should_throw_inner_exception_CoinTossAlreadyHappenedException()
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = CreateGame();
            var host = CreatePlayer(HostId, true);
            var challenger = CreatePlayer(ChallengerId, false);

            await context.Players.AddAsync(host);
            await context.Players.AddAsync(challenger);
            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var command = new GameStart.Request()
            {
                GameId = game.GameId
            };

            var coinTossHandler = new Mock<IStepHandler<CoinTossStep>>();
            coinTossHandler
                .Setup(x => x.ValidateAndThrow(It.IsAny<CoinTossStep>()))
                .Throws(new CoinTossAlreadyHappenedException(
                    new GameData(),
                    true));

            var createBoardHandler = new Mock<IStepHandler<CreateBoardStep>>();

            var subject = new GameStart.RequestHandler(
                context,
                coinTossHandler.Object,
                createBoardHandler.Object);

            Func<Task> act = async () => await subject.Handle(command, default);

            act.Should()
                .Throw<GameDataInvalidException>()
                .Where(e => e.GameId == GameId)
                .WithInnerException<CoinTossAlreadyHappenedException>();
        }
    }
}