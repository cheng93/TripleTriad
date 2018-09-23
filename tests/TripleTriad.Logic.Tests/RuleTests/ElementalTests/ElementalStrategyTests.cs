using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using TripleTriad.Logic.Cards;
using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Enums;
using TripleTriad.Logic.Extensions;
using TripleTriad.Logic.Rules;
using TripleTriad.Logic.Rules.Elemental;
using Xunit;

namespace TripleTriad.Logic.Tests.RuleTests.ElementalTests
{
    public class ElementalStrategyTests
    {
        private static int TileId = 0;

        public static IEnumerable<object[]> NoBoost()
        {
            var cards = new[]
            {
                AllCards.Shiva,
                AllCards.Ifrit,
                AllCards.Eden
            };

            foreach (var card in cards)
            {

                yield return new object[]
                {
                    Enumerable.Range(0, 9)
                        .Select(x => new Tile
                        {
                            TileId = x,
                            Card = new TileCard(card, true)
                        })
                };
            }
        }

        [Theory]
        [MemberData(nameof(NoBoost))]
        public void Should_not_boost_rank(IEnumerable<Tile> tiles)
        {
            var inner = new Mock<IRuleStrategy>();
            inner
                .Setup(x => x.Apply(
                    It.IsAny<IEnumerable<Tile>>(),
                    It.IsAny<int>()))
                .Returns<IEnumerable<Tile>, int>((x, y) => x);

            var subject = new ElementalStrategy(inner.Object);
            var result = subject.Apply(tiles, TileId);

            var tile = result.Single(x => x.TileId == TileId);
            tile.Card.Modifier.Should().Be(0);
        }

        public static IEnumerable<object[]> PositiveBoost = new[]
        {
            new object[]
            {
                Enumerable.Range(0, 9)
                    .Select(x => new Tile
                    {
                        TileId = x,
                        Card = new TileCard(AllCards.Shiva, true),
                        Element = x == TileId ? (Element?)Element.Ice : null
                    })
            },
            new object[]
            {
                Enumerable.Range(0, 9)
                    .Select(x => new Tile
                    {
                        TileId = x,
                        Card = new TileCard(AllCards.Ifrit, true),
                        Element = x == TileId ? (Element?)Element.Fire : null
                    })
            }
        };

        [Theory]
        [MemberData(nameof(PositiveBoost))]
        public void Should_positively_boost_rank(IEnumerable<Tile> tiles)
        {
            var inner = new Mock<IRuleStrategy>();
            inner
                .Setup(x => x.Apply(
                    It.IsAny<IEnumerable<Tile>>(),
                    It.IsAny<int>()))
                .Returns<IEnumerable<Tile>, int>((x, y) => x);

            var subject = new ElementalStrategy(inner.Object);
            var result = subject.Apply(tiles, TileId);

            var tile = result.Single(x => x.TileId == TileId);
            tile.Card.Modifier.Should().Be(1);
        }

        public static IEnumerable<object[]> NegativeBoost = new[]
        {
            new object[]
            {
                Enumerable.Range(0, 9)
                    .Select(x => new Tile
                    {
                        TileId = x,
                        Card = new TileCard(AllCards.Ifrit, true),
                        Element = x == TileId ? (Element?)Element.Ice : null
                    })
            },
            new object[]
            {
                Enumerable.Range(0, 9)
                    .Select(x => new Tile
                    {
                        TileId = x,
                        Card = new TileCard(AllCards.Shiva, true),
                        Element = x == TileId ? (Element?)Element.Fire : null
                    })
            },
            new object[]
            {
                Enumerable.Range(0, 9)
                    .Select(x => new Tile
                    {
                        TileId = x,
                        Card = new TileCard(AllCards.Eden, true),
                        Element = x == TileId ? (Element?)Element.Fire : null
                    })
            }
        };

        [Theory]
        [MemberData(nameof(NegativeBoost))]
        public void Should_negatively_boost_rank(IEnumerable<Tile> tiles)
        {
            var inner = new Mock<IRuleStrategy>();
            inner
                .Setup(x => x.Apply(
                    It.IsAny<IEnumerable<Tile>>(),
                    It.IsAny<int>()))
                .Returns<IEnumerable<Tile>, int>((x, y) => x);

            var subject = new ElementalStrategy(inner.Object);
            var result = subject.Apply(tiles, TileId);

            var tile = result.Single(x => x.TileId == TileId);
            tile.Card.Modifier.Should().Be(-1);
        }

        [Theory]
        [MemberData(nameof(NoBoost))]
        [MemberData(nameof(PositiveBoost))]
        [MemberData(nameof(NegativeBoost))]
        public void Should_call_inner_always(IEnumerable<Tile> tiles)
        {
            var inner = new Mock<IRuleStrategy>();
            inner
                .Setup(x => x.Apply(
                    It.IsAny<IEnumerable<Tile>>(),
                    It.IsAny<int>()))
                .Verifiable();

            var subject = new ElementalStrategy(inner.Object);
            subject.Apply(tiles, TileId);

            inner.Verify(x => x.Apply(
                It.IsAny<IEnumerable<Tile>>(),
                TileId));
        }
    }
}