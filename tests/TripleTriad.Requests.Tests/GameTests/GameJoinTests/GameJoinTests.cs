using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TripleTriad.Data.Entities;
using TripleTriad.Logic.Entities;
using TripleTriad.Requests.GameRequests;
using TripleTriad.Requests.GameRequests.Exceptions;
using TripleTriad.Requests.Tests.Utils;
using Xunit;

namespace TripleTriad.Requests.Tests.GameTests.GameJoinTests
{
    public class GameJoinTests
    {
        private readonly Guid playerId = Guid.NewGuid();

        private Game CreateGame() => new Game()
        {
            PlayerOneId = Guid.NewGuid(),
            Data = JsonConvert.SerializeObject(new GameData())
        };

        [Fact]
        public async Task Should_return_joined_game()
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = this.CreateGame();

            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var command = new GameJoin.Request()
            {
                GameId = game.GameId,
                PlayerId = this.playerId
            };
            var subject = new GameJoin.RequestHandler(context);

            var response = await subject.Handle(command, default);

            response.GameId.Should().Be(game.GameId);
        }

        [Fact]
        public async Task Should_have_correct_player_two_id()
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = this.CreateGame();

            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var command = new GameJoin.Request()
            {
                GameId = game.GameId,
                PlayerId = this.playerId
            };
            var subject = new GameJoin.RequestHandler(context);

            var response = await subject.Handle(command, default);

            game = await context.Games.SingleAsync(x => x.GameId == response.GameId);

            game.PlayerTwoId.Should().Be(this.playerId);
        }

        [Fact]
        public void Should_throw_GameNotFoundException()
        {
            var context = DbContextFactory.CreateTripleTriadContext();

            var command = new GameJoin.Request()
            {
                GameId = 1,
                PlayerId = this.playerId
            };
            var subject = new GameJoin.RequestHandler(context);

            Func<Task> act = async () => await subject.Handle(command, default);

            act.Should().Throw<GameNotFoundException>();
        }

        [Fact]
        public async Task Should_throw_CannotJoinGameException()
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = this.CreateGame();
            game.PlayerTwoId = Guid.NewGuid();

            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var command = new GameJoin.Request()
            {
                GameId = game.GameId,
                PlayerId = this.playerId
            };

            var subject = new GameJoin.RequestHandler(context);

            Func<Task> act = async () => await subject.Handle(command, default);

            act.Should().Throw<CannotJoinGameException>();
        }
    }
}