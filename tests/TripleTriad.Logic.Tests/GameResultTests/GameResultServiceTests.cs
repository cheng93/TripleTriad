using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using TripleTriad.Logic.Cards;
using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Enums;
using TripleTriad.Logic.GameResult;
using Xunit;

namespace TripleTriad.Logic.Tests.GameResultTests
{
    public class GameResultServiceTests
    {
        private static IEnumerable<Card> Cards = new[]
        {
            AllCards.Squall,
            AllCards.Rinoa,
            AllCards.Quistis,
            AllCards.Zell,
            AllCards.Selphie,
            AllCards.Irvine,
            AllCards.Laguna,
            AllCards.Kiros,
            AllCards.Ward
        };

        public static IEnumerable<object[]> ReturnNull()
        {
            yield return new object[] { new GameData() };
            for (var i = 0; i < 8; i++)
            {
                foreach (var coinTossWinner in new[] { true, false })
                {
                    yield return new object[]
                    {
                        new GameData
                        {
                            HostWonCoinToss = coinTossWinner,
                            Tiles = Enumerable.Range(0, 9)
                                .Select(x => new Tile
                                {
                                    TileId = x,
                                    Card = x < i
                                        ? new TileCard(
                                            Cards.Skip(x).First(),
                                            x % 2 == (coinTossWinner ? 0 : 1))
                                        : null
                                })
                        }
                    };
                }
            }
        }

        [Theory]
        [MemberData(nameof(ReturnNull))]
        public void Should_return_null(GameData gameData)
        {
            var subject = new GameResultService();

            var actual = subject.GetResult(gameData);
            actual.Should().BeNull();
        }

        public static IEnumerable<object[]> PlayerWin()
        {
            for (var i = 4; i < 9; i++)
            {
                foreach (var coinTossWinner in new[] { true, false })
                {
                    if (coinTossWinner && i == 4)
                    {
                        continue;
                    }

                    yield return new object[]
                    {
                        new GameData
                        {
                            HostWonCoinToss = coinTossWinner,
                            Tiles = Enumerable.Range(0, 9)
                                .Select(x => new Tile
                                {
                                    TileId = x,
                                    Card = new TileCard(
                                        Cards.Skip(x).First(),
                                        x <= i)
                                })
                        },
                        Result.HostWin
                    };

                    yield return new object[]
                    {
                        new GameData
                        {
                            HostWonCoinToss = coinTossWinner,
                            Tiles = Enumerable.Range(0, 9)
                                .Select(x => new Tile
                                {
                                    TileId = x,
                                    Card = new TileCard(
                                        Cards.Skip(x).First(),
                                        x >= i)
                                })
                        },
                        Result.PlayerTwoWin
                    };
                }
            }
        }

        [Theory]
        [MemberData(nameof(PlayerWin))]
        public void Should_return_player_one_win(GameData gameData, Result result)
        {
            var subject = new GameResultService();

            var actual = subject.GetResult(gameData);
            actual.Should().Be(result);
        }

        public static IEnumerable<object[]> Tie()
        {
            foreach (var coinTossWinner in new[] { true, false })
            {
                yield return new object[]
                {
                    new GameData
                    {
                        HostWonCoinToss = coinTossWinner,
                        Tiles = Enumerable.Range(0, 9)
                            .Select(x => new Tile
                            {
                                TileId = x,
                                Card = new TileCard(
                                    Cards.Skip(x).First(),
                                    x % 2 == (coinTossWinner ? 0 : 1))
                            })
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(Tie))]
        public void Should_return_tie(GameData gameData)
        {
            var subject = new GameResultService();

            var actual = subject.GetResult(gameData);
            actual.Should().Be(Result.Tie);
        }
    }
}