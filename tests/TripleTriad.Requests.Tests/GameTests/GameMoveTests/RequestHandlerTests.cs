using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using TripleTriad.Data.Entities;
using TripleTriad.Data.Enums;
using TripleTriad.Logic.Cards;
using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Extensions;
using TripleTriad.Requests.Exceptions;
using TripleTriad.Requests.GameRequests;
using TripleTriad.Requests.Tests.Utils;
using Xunit;

namespace TripleTriad.Requests.Tests.GameTests.GameMoveTests
{
    public class RequestHandlerTests
    {
        private static readonly Guid PlayerOneId = Guid.NewGuid();
        private static readonly Guid PlayerTwoId = Guid.NewGuid();
        private static readonly int GameId = 2;
        private static readonly string Card = AllCards.Seifer.Name;
        private static readonly int TileId = 3;

        private static Game CreateGame(GameData gameData = null)
        {
            gameData = gameData ?? new GameData();

            return new Game()
            {
                GameId = GameId,
                PlayerOneId = PlayerOneId,
                PlayerTwoId = PlayerTwoId,
                Status = GameStatus.InProgress,
                Data = gameData.ToJson()
            };
        }

        [Fact]
        public void Should_throw_GameNotFoundException()
        {
            var context = DbContextFactory.CreateTripleTriadContext();

            var command = new GameMove.Request()
            {
                GameId = GameId,
                PlayerId = PlayerOneId,
                Card = Card,
                TileId = TileId
            };

            // var selectCardsHandler = new Mock<IStepHandler<SelectCardsStep>>();

            // var subject = new GameMove.RequestHandler(context, selectCardsHandler.Object);
            var subject = new GameMove.RequestHandler(context);

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
                yield return new object[] { status, PlayerOneId };
                yield return new object[] { status, PlayerTwoId };
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
                PlayerId = PlayerOneId,
                Card = Card,
                TileId = TileId
            };

            // var selectCardsHandler = new Mock<IStepHandler<SelectCardsStep>>();

            // var subject = new GameMove.RequestHandler(context, selectCardsHandler.Object);
            var subject = new GameMove.RequestHandler(context);

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

            // var selectCardsHandler = new Mock<IStepHandler<SelectCardsStep>>();

            // var subject = new GameMove.RequestHandler(context, selectCardsHandler.Object);
            var subject = new GameMove.RequestHandler(context);

            Func<Task> act = async () => await subject.Handle(command, default);

            act.Should()
                .Throw<GameInvalidPlayerException>()
                .Where(x => x.GameId == GameId
                    && x.PlayerId == playerId);
        }
    }
}