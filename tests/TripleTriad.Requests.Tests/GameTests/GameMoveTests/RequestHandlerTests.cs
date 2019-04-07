using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using TripleTriad.Data.Entities;
using TripleTriad.Data.Enums;
using TripleTriad.Logic.Cards;
using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Enums;
using TripleTriad.Logic.Exceptions;
using TripleTriad.Logic.Extensions;
using TripleTriad.Logic.Steps;
using TripleTriad.Logic.Steps.Handlers;
using TripleTriad.Requests.Exceptions;
using TripleTriad.Requests.GameRequests;
using TripleTriad.Requests.Tests.Utils;
using Xunit;

namespace TripleTriad.Requests.Tests.GameTests.GameMoveTests
{
    public class RequestHandlerTests
    {
        private static readonly Guid HostId = Guid.NewGuid();
        private static readonly Guid ChallengerId = Guid.NewGuid();
        private static readonly int GameId = 2;
        private static readonly string Card = AllCards.Seifer.Name;
        private static readonly int TileId = 3;

        private static Tile CreateTile(bool isHost = true)
            => new Tile
            {
                TileId = TileId,
                Card = new TileCard(AllCards.Seifer, isHost)
            };

        private static readonly IEnumerable<Card> HostCards = new[]
        {
            AllCards.Squall,
            AllCards.Zell,
            AllCards.Edea,
            AllCards.Rinoa
        };

        private static readonly IEnumerable<Card> ChallengerCards = new[]
        {
            AllCards.Laguna,
            AllCards.Kiros,
            AllCards.Ward,
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
                Status = GameStatus.InProgress,
                Data = gameData.ToJson()
            };
        }

        [Fact]
        public async Task Should_return_game_id()
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = CreateGame();

            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var request = new GameMove.Request()
            {
                GameId = GameId,
                PlayerId = HostId,
                Card = Card,
                TileId = TileId
            };

            var playCardHandler = new Mock<IStepHandler<PlayCardStep>>();
            playCardHandler
                .Setup(x => x.Run(It.IsAny<PlayCardStep>()))
                .Returns(new GameData
                {
                    HostCards = HostCards,
                    ChallengerCards = ChallengerCards,
                    Tiles = new[] { CreateTile() }
                });

            var subject = new GameMove.RequestHandler(context, playCardHandler.Object);

            var response = await subject.Handle(request, default);

            response.GameId.Should().Be(GameId);
        }

        [Fact]
        public async Task Should_return_board()
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = CreateGame();

            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var request = new GameMove.Request()
            {
                GameId = GameId,
                PlayerId = HostId,
                Card = Card,
                TileId = TileId
            };

            var tile = CreateTile();

            var playCardHandler = new Mock<IStepHandler<PlayCardStep>>();
            playCardHandler
                .Setup(x => x.Run(It.IsAny<PlayCardStep>()))
                .Returns(new GameData
                {
                    HostCards = HostCards,
                    ChallengerCards = ChallengerCards,
                    Tiles = new[] { tile }
                });

            var subject = new GameMove.RequestHandler(context, playCardHandler.Object);

            var response = await subject.Handle(request, default);

            response.Tiles.Should().BeEquivalentTo(new[] { tile });
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Should_return_correct_cards(bool isHost)
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = CreateGame();

            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var request = new GameMove.Request()
            {
                GameId = GameId,
                PlayerId = isHost ? HostId : ChallengerId,
                Card = Card,
                TileId = TileId
            };

            var playCardHandler = new Mock<IStepHandler<PlayCardStep>>();
            playCardHandler
                .Setup(x => x.Run(It.IsAny<PlayCardStep>()))
                .Returns(new GameData
                {
                    HostCards = HostCards,
                    ChallengerCards = ChallengerCards,
                    Tiles = new[] { CreateTile(isHost) }
                });

            var subject = new GameMove.RequestHandler(context, playCardHandler.Object);

            var response = await subject.Handle(request, default);

            var cards = isHost ? HostCards : ChallengerCards;

            response.Cards.Should().BeEquivalentTo(cards);
        }

        [Theory]
        [InlineData(Result.HostWin)]
        [InlineData(Result.ChallengerWin)]
        [InlineData(Result.Tie)]
        [InlineData(Result.Cancelled)]
        public async Task Should_set_status_to_finshed(Result result)
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = CreateGame();

            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var request = new GameMove.Request()
            {
                GameId = GameId,
                PlayerId = HostId,
                Card = Card,
                TileId = TileId
            };

            var tile = CreateTile();

            var playCardHandler = new Mock<IStepHandler<PlayCardStep>>();
            playCardHandler
                .Setup(x => x.Run(It.IsAny<PlayCardStep>()))
                .Returns(new GameData
                {
                    HostCards = HostCards,
                    ChallengerCards = ChallengerCards,
                    Tiles = new[] { tile },
                    Result = result
                });

            var subject = new GameMove.RequestHandler(context, playCardHandler.Object);

            var response = await subject.Handle(request, default);

            game.Status.Should().Be(GameStatus.Finished);
        }

        [Fact]
        public void Should_throw_GameNotFoundException()
        {
            var context = DbContextFactory.CreateTripleTriadContext();

            var command = new GameMove.Request()
            {
                GameId = GameId,
                PlayerId = HostId,
                Card = Card,
                TileId = TileId
            };

            var playCardHandler = new Mock<IStepHandler<PlayCardStep>>();

            var subject = new GameMove.RequestHandler(context, playCardHandler.Object);

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
                GameStatus.ChooseCards,
                GameStatus.Finished
            };
            foreach (var status in badStatuses)
            {
                yield return new object[] { status, HostId };
                yield return new object[] { status, ChallengerId };
            }
        }

        [Theory]
        [InlineData(GameStatus.Waiting)]
        [InlineData(GameStatus.ChooseCards)]
        [InlineData(GameStatus.Finished)]
        public async Task Should_throw_GameHasInvalidStatusException(GameStatus status)
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = CreateGame();
            game.Status = status;

            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var command = new GameMove.Request()
            {
                GameId = GameId,
                PlayerId = HostId,
                Card = Card,
                TileId = TileId
            };

            var playCardHandler = new Mock<IStepHandler<PlayCardStep>>();

            var subject = new GameMove.RequestHandler(context, playCardHandler.Object);

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

            var command = new GameMove.Request()
            {
                GameId = GameId,
                PlayerId = playerId,
                Card = Card,
                TileId = TileId
            };

            var playCardHandler = new Mock<IStepHandler<PlayCardStep>>();

            var subject = new GameMove.RequestHandler(context, playCardHandler.Object);

            Func<Task> act = async () => await subject.Handle(command, default);

            act.Should()
                .Throw<GameInvalidPlayerException>()
                .Where(x => x.GameId == GameId
                    && x.PlayerId == playerId);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Should_throw_inner_exception_CardNotInHandException(bool isHost)
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = CreateGame();

            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var request = new GameMove.Request()
            {
                GameId = GameId,
                PlayerId = isHost ? HostId : ChallengerId,
                Card = Card,
                TileId = TileId
            };

            var playCardHandler = new Mock<IStepHandler<PlayCardStep>>();
            playCardHandler
                .Setup(x => x.ValidateAndThrow(It.IsAny<PlayCardStep>()))
                .Throws(new CardNotInHandException(new GameData(), isHost, Card));

            var subject = new GameMove.RequestHandler(context, playCardHandler.Object);

            Func<Task> act = async () => await subject.Handle(request, default);

            act.Should()
                .Throw<GameDataInvalidException>()
                .Where(e => e.GameId == GameId)
                .WithInnerException<CardNotInHandException>()
                .Where(e => e.IsHost == isHost
                    && e.Card == Card);
        }

        [Fact]
        public async Task Should_throw_inner_exception_TileUnavailableException()
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = CreateGame();

            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var request = new GameMove.Request()
            {
                GameId = GameId,
                PlayerId = HostId,
                Card = Card,
                TileId = TileId
            };

            var playCardHandler = new Mock<IStepHandler<PlayCardStep>>();
            playCardHandler
                .Setup(x => x.ValidateAndThrow(It.IsAny<PlayCardStep>()))
                .Throws(new TileUnavailableException(new GameData(), TileId));

            var subject = new GameMove.RequestHandler(context, playCardHandler.Object);

            Func<Task> act = async () => await subject.Handle(request, default);

            act.Should()
                .Throw<GameDataInvalidException>()
                .Where(e => e.GameId == GameId)
                .WithInnerException<TileUnavailableException>()
                .Where(e => e.TileId == TileId);
        }

        [Fact]
        public async Task Should_throw_inner_exception_NotPlayerTurnException()
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = CreateGame();

            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var request = new GameMove.Request()
            {
                GameId = GameId,
                PlayerId = HostId,
                Card = Card,
                TileId = TileId
            };

            var playCardHandler = new Mock<IStepHandler<PlayCardStep>>();
            playCardHandler
                .Setup(x => x.ValidateAndThrow(It.IsAny<PlayCardStep>()))
                .Throws(new NotPlayerTurnException(new GameData(), true));

            var subject = new GameMove.RequestHandler(context, playCardHandler.Object);

            Func<Task> act = async () => await subject.Handle(request, default);

            act.Should()
                .Throw<GameDataInvalidException>()
                .Where(e => e.GameId == GameId)
                .WithInnerException<NotPlayerTurnException>();
        }
    }
}