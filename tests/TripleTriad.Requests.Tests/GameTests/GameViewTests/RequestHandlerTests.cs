using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using TripleTriad.Data.Entities;
using TripleTriad.Requests.Exceptions;
using TripleTriad.Requests.GameRequests;
using TripleTriad.Requests.Tests.Utils;
using Xunit;

namespace TripleTriad.Requests.Tests.GameTests.GameViewTests
{
    public class RequestHandlerTests
    {
        private static readonly int GameId = 2;
        private static readonly Guid HostId = new Guid("3ede675b-cc81-42cd-bf13-ff6e8ab78b3d");
        private static readonly Guid ChallengerId = new Guid("403527f1-cdbb-4628-a9b0-25fc392fefc4");
        private static readonly Guid NonPlayerId = new Guid("c06c5fdd-63b5-4e6a-aa19-924bbddb6b79");

        private static Game CreateGame(Guid? hostId = null) => new Game()
        {
            GameId = GameId,
            HostId = HostId,
            ChallengerId = ChallengerId
        };

        public static IEnumerable<object[]> PlayerIds = new[]
        {
            new object[] { HostId },
            new object[] { ChallengerId },
            new object[] { NonPlayerId }
        };

        public static IEnumerable<object[]> PlayerIdsWithResult = new[]
        {
            new object[] { HostId, true, false },
            new object[] { ChallengerId, false, true },
            new object[] { NonPlayerId, false, false }
        };

        [Theory]
        [MemberData(nameof(PlayerIdsWithResult))]
        public async Task Should_return_response(Guid playerId, bool isHost, bool isChallenger)
        {
            var context = DbContextFactory.CreateTripleTriadContext();
            var game = CreateGame();

            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var command = new GameView.Request()
            {
                GameId = GameId,
                PlayerId = playerId
            };
            var subject = new GameView.RequestHandler(context);

            var response = await subject.Handle(command, default);

            response.IsHost.Should().Be(isHost);
            response.IsChallenger.Should().Be(isChallenger);
        }

        [Theory]
        [MemberData(nameof(PlayerIds))]
        public void Should_throw_GameNotFoundException(Guid playerId)
        {
            var gameId = 1;
            var context = DbContextFactory.CreateTripleTriadContext();

            var command = new GameView.Request()
            {
                GameId = gameId,
                PlayerId = playerId
            };
            var subject = new GameView.RequestHandler(context);

            Func<Task> act = async () => await subject.Handle(command, default);

            act.Should()
                .Throw<GameNotFoundException>()
                .Where(e => e.GameId == gameId);
        }
    }
}