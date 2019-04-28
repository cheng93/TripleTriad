using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using TripleTriad.Data.Entities;
using TripleTriad.Data.Enums;
using TripleTriad.Logic.Cards;
using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Extensions;
using TripleTriad.Requests.Messages.GameStateDataStrategies;
using Xunit;

namespace TripleTriad.Requests.Tests.MessageTests.GameStateDataStrategiesTests
{
    public class ChooseCardsGameStateDataStrategyTests
    {
        private static readonly Guid HostId = Guid.NewGuid();
        private static readonly Guid ChallengerId = Guid.NewGuid();
        private static readonly Guid NonPlayerId = Guid.NewGuid();
        private static readonly int GameId = 2;

        private static readonly IEnumerable<Card> HostCards = new[]
        {
            AllCards.Squall,
            AllCards.Zell,
            AllCards.Edea,
            AllCards.Rinoa,
            AllCards.Seifer
        };

        private static readonly IEnumerable<Card> ChallengerCards = new[]
        {
            AllCards.Laguna,
            AllCards.Kiros,
            AllCards.Ward,
            AllCards.Quistis,
            AllCards.Selphie
        };

        private static Game CreateGame(bool hostHasCards, bool challengerHasCards)
        {
            var gameData = new GameData
            {
                HostCards = hostHasCards
                    ? HostCards
                    : Enumerable.Empty<Card>(),
                ChallengerCards = challengerHasCards
                    ? ChallengerCards
                    : Enumerable.Empty<Card>()
            };

            return new Game()
            {
                GameId = GameId,
                Status = GameStatus.ChooseCards,
                HostId = HostId,
                ChallengerId = ChallengerId,
                Data = gameData.ToJson()
            };
        }

        public static IEnumerable<object[]> GetNonPlayerData()
        {
            var nonPlayerIds = new[]
            {
                (Guid?)null,
                NonPlayerId
            };

            foreach (var nonPlayerId in nonPlayerIds)
            {
                foreach (var hostHasCards in new[] { true, false })
                {
                    foreach (var challengerHasCards in new[] { true, false })
                    {
                        yield return new object[]
                        {
                            nonPlayerId,
                            hostHasCards,
                            challengerHasCards
                        };
                    }
                }
            }
        }

        public static IEnumerable<object[]> GetPlayerHasNoCardsData()
        {
            foreach (var b in new[] { true, false })
            {
                yield return new object[]
                {
                    HostId,
                    false,
                    b
                };
                yield return new object[]
                {
                    ChallengerId,
                    b,
                    false
                };
            }
        }

        public static IEnumerable<object[]> GetPlayerHasCardsData()
        {
            foreach (var b in new[] { true, false })
            {
                yield return new object[]
                {
                    HostId,
                    true,
                    b
                };
                yield return new object[]
                {
                    ChallengerId,
                    b,
                    true
                };
            }
        }

        [Theory]
        [MemberData(nameof(GetNonPlayerData))]
        [MemberData(nameof(GetPlayerHasCardsData))]
        [MemberData(nameof(GetPlayerHasNoCardsData))]
        public void Should_return_correct_state(Guid? playerId, bool hostHasCards, bool challengerHasCards)
        {
            var game = CreateGame(hostHasCards, challengerHasCards);

            var subject = new ChooseCards.GameStateDataStrategy();

            var actual = subject.GetData(game, playerId);

            actual.Should().Match(
                (GameStateData x) => x.GameId == GameId
                    && x.Status == "ChooseCards");
        }

        [Theory]
        [MemberData(nameof(GetNonPlayerData))]
        [MemberData(nameof(GetPlayerHasNoCardsData))]
        public void Should_not_return_cards(Guid? playerId, bool hostHasCards, bool challengerHasCards)
        {
            var game = CreateGame(hostHasCards, challengerHasCards);

            var subject = new ChooseCards.GameStateDataStrategy();

            var actual = subject.GetData(game, playerId);

            actual.Should().BeOfType(typeof(ChooseCards.Data));
            ((ChooseCards.Data)actual).SelectedCards.Should().BeEmpty();
        }

        [Theory]
        [MemberData(nameof(GetPlayerHasCardsData))]
        public void Should_return_correct_cards(Guid? playerId, bool hostHasCards, bool challengerHasCards)
        {
            var game = CreateGame(hostHasCards, challengerHasCards);

            var subject = new ChooseCards.GameStateDataStrategy();

            var actual = subject.GetData(game, playerId);

            var cards = playerId == HostId
                ? HostCards
                : ChallengerCards;

            var expected = cards.Select(x => x.Name);

            actual.Should().BeOfType(typeof(ChooseCards.Data));
            ((ChooseCards.Data)actual).SelectedCards.Should().BeEquivalentTo(expected);
        }
    }
}