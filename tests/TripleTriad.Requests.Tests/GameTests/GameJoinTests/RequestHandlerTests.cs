using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TripleTriad.Data.Entities;
using TripleTriad.Data.Enums;
using TripleTriad.Logic.Entities;
using TripleTriad.Requests.GameRequests;
using TripleTriad.Requests.Exceptions;
using TripleTriad.Requests.Tests.Utils;
using Xunit;

namespace TripleTriad.Requests.Tests.GameTests.GameJoinTests
{
    public class RequestHandlerTests
    {
        private static readonly int GameId = 2;
        private static readonly Guid PlayerId = Guid.NewGuid();

        private static Game CreateGame(Guid? playerOneId = null) => new Game()
        {
            GameId = GameId,
            PlayerOneId = playerOneId ?? Guid.NewGuid(),
            Data = JsonConvert.SerializeObject(new GameData())
        };

        [Fact]
        public async Task Should_return_joined_game()
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = CreateGame();

            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var command = new GameJoin.Request()
            {
                GameId = GameId,
                PlayerId = PlayerId
            };
            var subject = new GameJoin.RequestHandler(context);

            var response = await subject.Handle(command, default);

            response.GameId.Should().Be(GameId);
        }

        [Fact]
        public async Task Should_have_correct_player_two_id()
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = CreateGame();

            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var command = new GameJoin.Request()
            {
                GameId = GameId,
                PlayerId = PlayerId
            };
            var subject = new GameJoin.RequestHandler(context);

            var response = await subject.Handle(command, default);

            game = await context.Games.SingleAsync(x => x.GameId == response.GameId);

            game.PlayerTwoId.Should().Be(PlayerId);
        }

        [Fact]
        public async Task Should_set_game_status_to_choose_cards()
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = CreateGame();

            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var command = new GameJoin.Request()
            {
                GameId = GameId,
                PlayerId = PlayerId
            };
            var subject = new GameJoin.RequestHandler(context);

            var response = await subject.Handle(command, default);

            game = await context.Games.SingleAsync(x => x.GameId == response.GameId);

            game.Status.Should().Be(GameStatus.ChooseCards);
        }

        [Fact]
        public void Should_throw_GameNotFoundException()
        {
            var gameId = 1;
            var context = DbContextFactory.CreateTripleTriadContext();

            var command = new GameJoin.Request()
            {
                GameId = gameId,
                PlayerId = PlayerId
            };
            var subject = new GameJoin.RequestHandler(context);

            Func<Task> act = async () => await subject.Handle(command, default);

            act.Should()
                .Throw<GameNotFoundException>()
                .Where(e => e.GameId == gameId);
        }

        [Fact]
        public async Task Should_throw_CannotJoinGameException()
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = CreateGame();
            game.PlayerTwoId = Guid.NewGuid();

            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var command = new GameJoin.Request()
            {
                GameId = GameId,
                PlayerId = PlayerId
            };

            var subject = new GameJoin.RequestHandler(context);

            Func<Task> act = async () => await subject.Handle(command, default);

            act.Should()
                .Throw<CannotJoinGameException>()
                .Where(e => e.GameId == GameId);
        }

        [Fact]
        public async Task Should_throw_CannotPlayYourselfException()
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = CreateGame(PlayerId);

            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var command = new GameJoin.Request()
            {
                GameId = GameId,
                PlayerId = PlayerId
            };

            var subject = new GameJoin.RequestHandler(context);

            Func<Task> act = async () => await subject.Handle(command, default);

            act.Should().Throw<CannotPlayYourselfException>()
                .Where(e => e.GameId == GameId && e.PlayerId == PlayerId);
        }
    }
}