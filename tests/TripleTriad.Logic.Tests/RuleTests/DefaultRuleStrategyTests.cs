using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using TripleTriad.Logic.Capture;
using TripleTriad.Logic.Cards;
using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Rules;
using Xunit;

namespace TripleTriad.Logic.Tests.RuleTests
{
    public class DefaultRuleStrategyTests
    {
        private static Card Card = AllCards.Seifer;

        private static IEnumerable<Tile> Tiles
            = Enumerable.Range(0, 9)
                .Select(x => new Tile
                {
                    TileId = x,
                    Card = new[] { 0, 1, 3 }.Contains(x)
                        ? new TileCard(Card, x == 0)
                        : null
                });

        public static IEnumerable<object[]> Captures = new[]
        {
            new object[] { new int[] {}, false, false},
            new object[] { new int[] {1}, true, false},
            new object[] { new int[] {3}, false, true},
            new object[] { new int[] {1,3}, true, true}
        };

        [Theory]
        [MemberData(nameof(Captures))]
        public void Should_change_card_status_of_captured_cards(
            IEnumerable<int> captures,
            bool captured1,
            bool captured3)
        {
            var captureService = new Mock<ICaptureService>();
            captureService
                .Setup(x => x.Captures(
                    It.IsAny<IEnumerable<Tile>>(),
                    It.IsAny<int>()))
                .Returns(captures);

            var subject = new DefaultRuleStrategy(captureService.Object);
            var tiles = subject.Apply(Tiles, 0);

            tiles
                .Single(x => x.TileId == 1)
                .Card
                .IsHost
                .Should()
                .Be(captured1);
            tiles
                .Single(x => x.TileId == 3)
                .Card
                .IsHost
                .Should()
                .Be(captured3);
        }
    }
}