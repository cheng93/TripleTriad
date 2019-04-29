using System;
using FluentAssertions;
using TripleTriad.Data.Entities;
using TripleTriad.Data.Enums;
using TripleTriad.Requests.Messages.GameStateDataStrategies;
using Xunit;

namespace TripleTriad.Requests.Tests.MessageTests.GameStateDataStrategiesTests
{
    public class GameStateDataStrategyFactoryTests
    {
        [Theory]
        [InlineData(GameStatus.Waiting, typeof(DefaultGameStateDataStrategy))]
        [InlineData(GameStatus.ChooseCards, typeof(ChooseCards.GameStateDataStrategy))]
        [InlineData(GameStatus.InProgress, typeof(InProgress.GameStateDataStrategy))]
        public void Should_be_of_type(GameStatus status, Type expected)
        {
            var game = new Game
            {
                Status = status
            };

            var subject = new GameStateDataStrategyFactory();

            var strategy = subject.GetStrategy(game);

            strategy.Should().BeOfType(expected);
        }
    }
}