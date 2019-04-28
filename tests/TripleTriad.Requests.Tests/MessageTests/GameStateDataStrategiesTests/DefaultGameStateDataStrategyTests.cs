using System;
using FluentAssertions;
using TripleTriad.Data.Entities;
using TripleTriad.Data.Enums;
using TripleTriad.Requests.Messages.GameStateDataStrategies;
using Xunit;

namespace TripleTriad.Requests.Tests.MessageTests.GameStateDataStrategiesTests
{
    public class DefaultGameStateDataStrategyTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("0d25184d-7dcb-4edf-bad6-ce415b46d21d")]
        public void Should_return_correct_state(string playerId)
        {
            var game = new Game
            {
                GameId = 2,
                Status = GameStatus.Waiting
            };

            var subject = new DefaultGameStateDataStrategy();

            var actual = subject.GetData(
                game,
                Guid.TryParse(playerId, out var g)
                    ? (Guid?)g
                    : null);

            actual.Should().Match(
                (GameStateData x) => x.GameId == 2
                    && x.Status == "Waiting");
        }
    }
}