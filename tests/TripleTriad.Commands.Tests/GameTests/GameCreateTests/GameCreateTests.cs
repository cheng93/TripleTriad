using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TripleTriad.Commands.Game;
using TripleTriad.Commands.Tests.Utils;
using TripleTriad.Logic.Entities;
using Xunit;

namespace TripleTriad.Commands.Tests.GameTests.GameCreateTests
{
    using Game = TripleTriad.Data.Entities.Game;

    public class GameCreateTests
    {
        private readonly Guid playerId = Guid.NewGuid();

        [Fact]
        public async Task Should_return_created_game()
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var command = new GameCreate.Request()
            {
                PlayerId = this.playerId
            };
            var subject = new GameCreate.RequestHandler(context);

            var response = await subject.Handle(command, default);

            Func<Task<Game>> act = async () => await context.Games.SingleAsync(x => x.GameId == response.GameId);

            act.Should().NotThrow();
        }

        [Fact]
        public async Task Should_have_correct_player_one_id()
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var command = new GameCreate.Request()
            {
                PlayerId = this.playerId
            };
            var subject = new GameCreate.RequestHandler(context);

            var response = await subject.Handle(command, default);

            var game = await context.Games.SingleAsync(x => x.GameId == response.GameId);

            game.PlayerOneId.Should().Be(this.playerId);
        }

        [Fact]
        public async Task Should_have_correct_new_game_data()
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var command = new GameCreate.Request()
            {
                PlayerId = this.playerId
            };
            var subject = new GameCreate.RequestHandler(context);

            var response = await subject.Handle(command, default);

            var game = await context.Games.SingleAsync(x => x.GameId == response.GameId);

            var expectedGameData = JsonConvert.SerializeObject(new GameData());

            JToken.DeepEquals(JObject.Parse(game.Data), JObject.Parse(expectedGameData))
                .Should()
                .BeTrue();
        }
    }
}