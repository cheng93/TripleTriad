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
using TripleTriad.Requests.Exceptions;
using TripleTriad.Requests.GameRequests;
using TripleTriad.Requests.Tests.Utils;
using Xunit;

namespace TripleTriad.Requests.Tests.GameTests.CardSelectTests
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
            gameData = gameData ?? new GameData();

            return new Game()
            {
                GameId = GameId,
                HostId = HostId,
                ChallengerId = ChallengerId,
                Status = GameStatus.ChooseCards,
                Data = gameData.ToJson()
            };
        }

        private static Player CreatePlayer(bool isHost)
        {
            return new Player
            {
                PlayerId = isHost ? HostId : ChallengerId,
                DisplayName = $"Player{(isHost ? 1 : 2)}"
            };
        }

        [Fact]
        public async Task Should_return_cards()
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = CreateGame();

            await context.Players.AddAsync(CreatePlayer(true));
            await context.Players.AddAsync(CreatePlayer(false));
            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var request = new CardSelect.Request()
            {
                GameId = GameId,
                PlayerId = HostId
            };

            var selectCardsHandler = new Mock<IStepHandler<SelectCardsStep>>();
            selectCardsHandler
                .Setup(x => x.Run(It.IsAny<SelectCardsStep>()))
                .Returns(new GameData
                {
                    HostCards = Cards
                });

            var subject = new CardSelect.RequestHandler(context, selectCardsHandler.Object);

            var response = await subject.Handle(request, default);

            response.Cards.Should().BeEquivalentTo(Cards);
        }

        public static IEnumerable<object[]> Requests = new[]
        {
            new object[]{ true, null, false},
            new object[]{ false, null, false},
            new object[]{ true, Cards, true},
            new object[]{ false, Cards, true}
        };

        [Theory]
        [MemberData(nameof(Requests))]
        public async Task Should_return_correct_queue_task(
            bool isHost,
            IEnumerable<Card> opponentCards,
            bool queueTask)
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = CreateGame();

            await context.Players.AddAsync(CreatePlayer(true));
            await context.Players.AddAsync(CreatePlayer(false));
            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var request = new CardSelect.Request()
            {
                GameId = GameId,
                PlayerId = isHost ? HostId : ChallengerId
            };

            var selectCardsHandler = new Mock<IStepHandler<SelectCardsStep>>();
            selectCardsHandler
                .Setup(x => x.Run(It.IsAny<SelectCardsStep>()))
                .Returns(new GameData
                {
                    HostCards = isHost ? Cards : opponentCards,
                    ChallengerCards = isHost ? opponentCards : Cards
                });

            var subject = new CardSelect.RequestHandler(context, selectCardsHandler.Object);

            var response = await subject.Handle(request, default);

            response.QueueTask.Should().Be(queueTask);
        }

        [Fact]
        public void Should_throw_GameNotFoundException()
        {
            var context = DbContextFactory.CreateTripleTriadContext();

            var command = new CardSelect.Request()
            {
                GameId = GameId,
                PlayerId = HostId
            };

            var selectCardsHandler = new Mock<IStepHandler<SelectCardsStep>>();

            var subject = new CardSelect.RequestHandler(context, selectCardsHandler.Object);

            Func<Task> act = async () => await subject.Handle(command, default);

            act.Should()
                .Throw<GameNotFoundException>()
                .Where(e => e.GameId == GameId);
        }

        public static IEnumerable<object[]> InvalidStatuses()
        {
            var badStatuses = new[]
            {
                GameStatus.Waiting,
                GameStatus.InProgress,
                GameStatus.Finished
            };
            foreach (var status in badStatuses)
            {
                yield return new object[] { status, HostId };
                yield return new object[] { status, ChallengerId };
            }
        }

        [Theory]
        [MemberData(nameof(InvalidStatuses))]
        public async Task Should_throw_GameHasInvalidStatusException(GameStatus status, Guid playerId)
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = CreateGame();
            game.Status = status;

            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var command = new CardSelect.Request()
            {
                GameId = GameId,
                PlayerId = playerId
            };

            var selectCardsHandler = new Mock<IStepHandler<SelectCardsStep>>();

            var subject = new CardSelect.RequestHandler(context, selectCardsHandler.Object);

            Func<Task> act = async () => await subject.Handle(command, default);

            act.Should()
                .Throw<GameHasInvalidStatusException>()
                .Where(x => x.GameId == GameId
                    && x.Status == status);
        }

        [Fact]
        public async Task Should_throw_GameInvalidPlayerException()
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = CreateGame();

            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var playerId = Guid.NewGuid();

            var command = new CardSelect.Request()
            {
                GameId = GameId,
                PlayerId = playerId
            };

            var selectCardsHandler = new Mock<IStepHandler<SelectCardsStep>>();

            var subject = new CardSelect.RequestHandler(context, selectCardsHandler.Object);

            Func<Task> act = async () => await subject.Handle(command, default);

            act.Should()
                .Throw<GameInvalidPlayerException>()
                .Where(x => x.GameId == GameId
                    && x.PlayerId == playerId);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Should_throw_inner_exception_CardsAlreadySelectedException(bool isHost)
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = CreateGame();

            await context.Players.AddAsync(CreatePlayer(true));
            await context.Players.AddAsync(CreatePlayer(false));
            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var playerId = isHost ? HostId : ChallengerId;

            var command = new CardSelect.Request()
            {
                GameId = GameId,
                PlayerId = playerId
            };

            var selectCardsHandler = new Mock<IStepHandler<SelectCardsStep>>();
            selectCardsHandler
                .Setup(x => x.ValidateAndThrow(It.IsAny<SelectCardsStep>()))
                .Throws(new CardsAlreadySelectedException(new GameData(), isHost));

            var subject = new CardSelect.RequestHandler(context, selectCardsHandler.Object);

            Func<Task> act = async () => await subject.Handle(command, default);

            act.Should()
                .Throw<GameDataInvalidException>()
                .Where(x => x.GameId == GameId)
                .WithInnerException<CardsAlreadySelectedException>();
        }
    }
}