using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TripleTriad.Data.Entities;
using TripleTriad.Data.Enums;
using TripleTriad.Requests.GameRequests;
using TripleTriad.Requests.Tests.Utils;
using Xunit;

namespace TripleTriad.Requests.Tests.GameTests.GameListTests
{
    public class RequestHandlerTests
    {
        [Fact]
        public async Task Should_return_game_ids_that_are_waiting()
        {
            var context = DbContextFactory.CreateTripleTriadContext();

            for (var i = 0; i < 4; i++)
            {
                var game = new Game
                {
                    Status = i % 2 == 0
                        ? GameStatus.Waiting
                        : (GameStatus)i
                };
                await context.Games.AddAsync(game);
            }

            await context.SaveChangesAsync();

            var request = new GameList.Request();

            var subject = new GameList.RequestHandler(context);

            var response = await subject.Handle(request, default);

            var gameStatuses = await context
                .Games
                .Where(x => response.GameIds.Contains(x.GameId))
                .Select(x => x.Status)
                .ToListAsync();

            gameStatuses.Should().AllBeEquivalentTo(GameStatus.Waiting);
        }

        [Fact]
        public async Task Should_return_ascending_game_ids()
        {
            var context = DbContextFactory.CreateTripleTriadContext();

            for (var i = 4; i > 0; i--)
            {
                var game = new Game
                {
                    GameId = 2 * i,
                    Status = GameStatus.Waiting
                };
                await context.Games.AddAsync(game);
            }

            await context.SaveChangesAsync();

            var request = new GameList.Request();

            var subject = new GameList.RequestHandler(context);

            var response = await subject.Handle(request, default);

            response.GameIds.Should().BeInAscendingOrder();
        }

        [Fact]
        public async Task Should_return_game_ids()
        {
            var context = DbContextFactory.CreateTripleTriadContext();

            for (var i = 0; i < 4; i++)
            {
                var game = new Game
                {
                    GameId = i + 1,
                    Status = i % 2 == 0
                        ? GameStatus.Waiting
                        : (GameStatus)i
                };
                await context.Games.AddAsync(game);
            }

            await context.SaveChangesAsync();

            var request = new GameList.Request();

            var subject = new GameList.RequestHandler(context);

            var response = await subject.Handle(request, default);

            response.GameIds.Should().BeEquivalentTo(new[] { 1, 3 });
        }
    }
}