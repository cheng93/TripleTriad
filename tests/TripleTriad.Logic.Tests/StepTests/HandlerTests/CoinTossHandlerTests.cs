using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using TripleTriad.Logic.Cards;
using TripleTriad.Logic.CoinToss;
using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Exceptions;
using TripleTriad.Logic.Steps;
using TripleTriad.Logic.Steps.Handlers;
using Xunit;

namespace TripleTriad.Logic.Tests.StepTests.HandlerTests
{
    public class CoinTossHandlerTests
    {
        private static readonly string HostDisplay = "Host";

        private static readonly string ChallengerDisplay = "Challenger";

        private static IEnumerable<Card> Cards = new[]
        {
            AllCards.Squall,
            AllCards.Seifer,
            AllCards.Edea,
            AllCards.Rinoa,
            AllCards.Quistis
        };

        private static CoinTossStep CreateStep(GameData gameData = null)
            => new CoinTossStep(gameData ?? new GameData(), HostDisplay, ChallengerDisplay);

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_have_correct_coin_toss_result(bool coinTossIsHeads)
        {
            var coinTossService = new Mock<ICoinTossService>();
            coinTossService
                .Setup(x => x.IsHeads())
                .Returns(coinTossIsHeads);

            var subject = new CoinTossHandler(coinTossService.Object);
            var data = subject.Run(CreateStep());

            data.HostWonCoinToss.Should().Be(coinTossIsHeads);
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

            var subject = new CoinTossHandler(coinTossService.Object);
            var data = subject.Run(CreateStep());

            data.HostTurn.Should().Be(coinTossIsHeads);
        }

        public static IEnumerable<object[]> ExpectedLogEntry => new[]
        {
            new object[] { true, $"{HostDisplay} won the coin toss." },
            new object[] { false, $"{ChallengerDisplay} won the coin toss." }
        };

        [Theory]
        [MemberData(nameof(ExpectedLogEntry))]
        public void Should_have_correct_log_entry(bool coinTossIsHeads, string logEntry)
        {
            var coinTossService = new Mock<ICoinTossService>();
            coinTossService
                .Setup(x => x.IsHeads())
                .Returns(coinTossIsHeads);

            var subject = new CoinTossHandler(coinTossService.Object);
            var data = subject.Run(CreateStep());

            data.Log.Last().Should().Be(logEntry);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_throw_CoinTossAlreadyHappenedException(bool hostWon)
        {
            var gameData = new GameData
            {
                HostWonCoinToss = hostWon
            };

            var coinTossService = new Mock<ICoinTossService>();
            var subject = new CoinTossHandler(coinTossService.Object);

            Action act = () => subject.ValidateAndThrow(CreateStep(gameData));

            act.Should()
                .Throw<CoinTossAlreadyHappenedException>()
                .Where(x => x.GameData == gameData
                    && x.HostWonCoinToss == hostWon);
        }

        public static IEnumerable<object[]> StillSelectingCards = new[]
        {
            new object[] { null, null, true, true },
            new object[] { Cards, null, false, true },
            new object[] { null, Cards, true, false }
        };

        [Theory]
        [MemberData(nameof(StillSelectingCards))]
        public void Should_throw_PlayerStillSelectingCardsException(
            IEnumerable<Card> hostCards,
            IEnumerable<Card> challengerCards,
            bool hostStillSelecting,
            bool challengerStillSelecting)
        {
            var gameData = new GameData
            {
                HostCards = hostCards,
                ChallengerCards = challengerCards
            };

            var coinTossService = new Mock<ICoinTossService>();
            var subject = new CoinTossHandler(coinTossService.Object);

            Action act = () => subject.ValidateAndThrow(CreateStep(gameData));

            act.Should()
                .Throw<PlayerStillSelectingCardsException>()
                .Where(x => x.GameData == gameData
                    && x.Host == hostStillSelecting
                    && x.Challenger == challengerStillSelecting);
        }
    }
}