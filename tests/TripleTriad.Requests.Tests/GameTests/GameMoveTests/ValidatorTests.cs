using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using TripleTriad.Logic.Cards;
using TripleTriad.Requests.GameRequests;
using Xunit;

namespace TripleTriad.Requests.Tests.GameTests.GameMoveTests
{
    public class ValidatorTests
    {
        public static IEnumerable<object[]> BadRequests()
        {
            yield return new object[] { new GameMove.Request() };

            var gameIds = new[] { 0, 2 };
            var playerIds = new[] { Guid.Empty, Guid.NewGuid() };
            var cards = new[] { AllCards.Seifer.Name, "BadCard" };
            var tileIds = Enumerable.Range(-1, 11);

            foreach (var playerId in playerIds)
            {
                foreach (var card in cards)
                {
                    foreach (var tileId in tileIds)
                    {
                        yield return new object[]
                        {
                            new GameMove.Request()
                            {
                                GameId = 0,
                                PlayerId = playerId,
                                Card = card,
                                TileId = tileId
                            }
                        };
                    }
                }
            }

            foreach (var gameId in gameIds)
            {
                foreach (var card in cards)
                {
                    foreach (var tileId in tileIds)
                    {
                        yield return new object[]
                        {
                            new GameMove.Request()
                            {
                                GameId = gameId,
                                PlayerId = Guid.Empty,
                                Card = card,
                                TileId = tileId
                            }
                        };
                    }
                }
            }

            foreach (var gameId in gameIds)
            {
                foreach (var playerId in playerIds)
                {
                    foreach (var tileId in tileIds)
                    {
                        yield return new object[]
                        {
                            new GameMove.Request()
                            {
                                GameId = gameId,
                                PlayerId = playerId,
                                Card = "BadCard",
                                TileId = tileId
                            }
                        };
                    }
                }
            }

            foreach (var gameId in gameIds)
            {
                foreach (var playerId in playerIds)
                {
                    foreach (var card in cards)
                    {
                        foreach (var tileId in new[] { -1, 9 })
                        {
                            yield return new object[]
                            {
                                new GameMove.Request()
                                {
                                    GameId = gameId,
                                    PlayerId = playerId,
                                    Card = card,
                                    TileId = tileId
                                }
                            };
                        }
                    }
                }
            }
        }

        [Theory]
        [MemberData(nameof(BadRequests))]
        public void Bad_request_should_throw(GameMove.Request request)
        {
            var subject = new GameMove.Validator();
            Func<Task> action = async () => await subject.Process(request, default);

            action.Should().Throw<ValidationException>();
        }

        public static IEnumerable<object[]> GoodRequests()
        {
            var tileIds = Enumerable.Range(0, 9);

            foreach (var tileId in tileIds)
            {
                yield return new object[]
                {
                    new GameMove.Request()
                    {
                        GameId = 2,
                        PlayerId = Guid.NewGuid(),
                        Card = AllCards.Seifer.Name,
                        TileId = tileId
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(GoodRequests))]
        public void Good_request_should_not_throw(GameMove.Request request)
        {
            var subject = new GameMove.Validator();
            Func<Task> action = async () => await subject.Process(request, default);

            action.Should().NotThrow<ValidationException>();
        }
    }
}