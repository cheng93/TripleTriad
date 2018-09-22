using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using TripleTriad.Logic.CoinToss;
using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Steps;
using TripleTriad.Logic.Steps.Strategies;
using Xunit;

namespace TripleTriad.Logic.Tests.StepTests.StrategyTests
{
    public class CoinTossStrategyTests
    {
        private static readonly string PlayerOneDisplay = "PlayerOne";

        private static readonly string PlayerTwoDisplay = "PlayerTwo";

        private static CoinTossStep CreateStep()
            => new CoinTossStep(new GameData(), PlayerOneDisplay, PlayerTwoDisplay);

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_have_correct_coin_toss_result(bool coinTossIsHeads)
        {
            var coinTossService = new Mock<ICoinTossService>();
            coinTossService
                .Setup(x => x.IsHeads())
                .Returns(coinTossIsHeads);

            var subject = new CoinTossStrategy(coinTossService.Object);
            var data = subject.Run(CreateStep());

            data.PlayerOneWonCoinToss.Should().Be(coinTossIsHeads);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_have_correct_player_step(bool coinTossIsHeads)
        {
            var coinTossService = new Mock<ICoinTossService>();
            coinTossService
                .Setup(x => x.IsHeads())
                .Returns(coinTossIsHeads);

            var subject = new CoinTossStrategy(coinTossService.Object);
            var data = subject.Run(CreateStep());

            data.PlayerOneTurn.Should().Be(coinTossIsHeads);
        }

        public static IEnumerable<object[]> ExpectedLogEntry => new[]
        {
            new object[] { true, $"{PlayerOneDisplay} won the coin toss." },
            new object[] { false, $"{PlayerTwoDisplay} won the coin toss." }
        };

        [Theory]
        [MemberData(nameof(ExpectedLogEntry))]
        public void Should_have_correct_log_entry(bool coinTossIsHeads, string logEntry)
        {
            var coinTossService = new Mock<ICoinTossService>();
            coinTossService
                .Setup(x => x.IsHeads())
                .Returns(coinTossIsHeads);

            var subject = new CoinTossStrategy(coinTossService.Object);
            var data = subject.Run(CreateStep());

            data.Log.Last().Should().Be(logEntry);
        }
    }
}