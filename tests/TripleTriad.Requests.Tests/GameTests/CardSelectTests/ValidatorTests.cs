using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using TripleTriad.Logic.Cards;
using TripleTriad.Requests.GameRequests;
using Xunit;

namespace TripleTriad.Requests.Tests.GameTests.CardSelectTests
{
    public class ValidatorTests
    {
        private static IEnumerable<string> Cards = new[]
        {
            AllCards.Squall,
            AllCards.Quistis,
            AllCards.Zell,
            AllCards.Selphie,
            AllCards.Rinoa,
            AllCards.Irvine
        }
        .Select(x => x.Name)
        .ToList();

        public static IEnumerable<object[]> BadRequests()
        {
            yield return new object[] { new CardSelect.Request() };
            yield return new object[] { new CardSelect.Request() { GameId = 0, PlayerId = Guid.Empty, Cards = Cards.Take(5) } };
            yield return new object[] { new CardSelect.Request() { GameId = 1, PlayerId = Guid.Empty, Cards = Cards.Take(5) } };
            yield return new object[] { new CardSelect.Request() { GameId = 0, PlayerId = Guid.NewGuid(), Cards = Cards.Take(5) } };

            for (var i = -1; i < 7; i++)
            {
                var cards = i == -1
                    ? null
                    : i != 5
                        ? Cards.Take(i)
                        : Cards.Take(4).Concat(new[] { "InvalidName" });
                yield return new object[]
                {
                    new CardSelect.Request()
                    {
                        GameId = 1,
                        PlayerId = Guid.NewGuid(),
                        Cards = cards
                    }
                };
                yield return new object[]
                {
                    new CardSelect.Request()
                    {
                        GameId = 0,
                        PlayerId = Guid.NewGuid(),
                        Cards = cards
                    }
                };
                yield return new object[]
                {
                    new CardSelect.Request()
                    {
                        GameId = 1,
                        PlayerId = Guid.Empty,
                        Cards = cards
                    }
                };
                yield return new object[]
                {
                    new CardSelect.Request()
                    {
                        GameId = 0,
                        PlayerId = Guid.Empty,
                        Cards = cards
                    }
                };

                if (Enumerable.Range(2, 4).Contains(i))
                {
                    cards = Enumerable.Range(0, i)
                        .Select(x => AllCards.Seifer.Name)
                        .Concat(Cards.Take(5 - i));

                    yield return new object[]
                    {
                        new CardSelect.Request()
                        {
                            GameId = 1,
                            PlayerId = Guid.NewGuid(),
                            Cards = cards
                        }
                    };

                    yield return new object[]
                    {
                        new CardSelect.Request()
                        {
                            GameId = 0,
                            PlayerId = Guid.NewGuid(),
                            Cards = cards
                        }
                    };
                    yield return new object[]
                    {
                        new CardSelect.Request()
                        {
                            GameId = 1,
                            PlayerId = Guid.Empty,
                            Cards = cards
                        }
                    };
                    yield return new object[]
                    {
                        new CardSelect.Request()
                        {
                            GameId = 0,
                            PlayerId = Guid.Empty,
                            Cards = cards
                        }
                    };
                }
            }
        }

        [Theory]
        [MemberData(nameof(BadRequests))]
        public void Bad_request_should_throw(CardSelect.Request request)
        {
            var subject = new CardSelect.Validator();
            Func<Task> action = async () => await subject.Process(request, default);

            action.Should().Throw<ValidationException>();
        }

        public static IEnumerable<object[]> GoodRequests => new[]
        {
            new object[]
            {
                new CardSelect.Request()
                {
                    GameId = 1,
                    PlayerId = Guid.NewGuid(),
                    Cards = Cards.Take(5)
                }
            },
            new object[]
            {
                new CardSelect.Request()
                {
                    GameId = 2,
                    PlayerId = new Guid("6faf20d7-eee5-482a-b8ff-73754f39cf19"),
                    Cards = Cards.Skip(1).Take(5)
                }
            }
        };

        [Theory]
        [MemberData(nameof(GoodRequests))]
        public void Good_request_should_not_throw(CardSelect.Request request)
        {
            var subject = new CardSelect.Validator();
            Func<Task> action = async () => await subject.Process(request, default);

            action.Should().NotThrow<ValidationException>();
        }
    }
}