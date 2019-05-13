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
    public class InProgressGameStateDataStrategyTests
    {
        private const string HostId = "96eca092-f4eb-49e2-b1e6-a03801892754";
        private const string ChallengerId = "d0e7cd39-3b02-466f-89aa-5bd130860b00";
        private const string NonPlayerId = "ebd5fa88-c13e-4c9d-ace7-b94fd055db94";
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
            AllCards.Quistis
        };

        private static Game CreateGame()
        {
            var gameData = new GameData
            {
                HostCards = HostCards,
                ChallengerCards = ChallengerCards,
                HostTurn = true,
                HostWonCoinToss = false
            };

            return new Game()
            {
                GameId = GameId,
                Status = GameStatus.InProgress,
                HostId = new Guid(HostId),
                ChallengerId = new Guid(ChallengerId),
                Data = gameData.ToJson()
            };
        }

        [Theory]
        [InlineData(HostId)]
        [InlineData(ChallengerId)]
        [InlineData(NonPlayerId)]
        [InlineData(null)]
        public void Should_return_correct_state(string playerId)
        {
            var game = CreateGame();

            var subject = new InProgress.GameStateDataStrategy();

            var actual = subject.GetData(game, Guid.TryParse(playerId, out var g) ? (Guid?)g : null);

            actual.Should().BeOfType(typeof(InProgress.Data));
            actual.Should().Match(
                (InProgress.Data x) => x.GameId == GameId
                    && x.Status == "InProgress"
                    && x.HostTurn == true
                    && x.HostWonCoinToss == false);
        }

        [Fact]
        public void Should_return_host_cards_when_player_id_is_host()
        {
            var game = CreateGame();

            var subject = new InProgress.GameStateDataStrategy();

            var actual = subject.GetData(game, new Guid(HostId));

            actual.Should().BeOfType(typeof(InProgress.Data));
            ((InProgress.Data)actual).HostCards
                .Should()
                .BeEquivalentTo(HostCards.Select(x => x.Name));
        }

        [Fact]
        public void Should_return_challenger_cards_when_player_id_is_challenger()
        {
            var game = CreateGame();

            var subject = new InProgress.GameStateDataStrategy();

            var actual = subject.GetData(game, new Guid(ChallengerId));

            actual.Should().BeOfType(typeof(InProgress.Data));
            ((InProgress.Data)actual).ChallengerCards
                .Should()
                .BeEquivalentTo(ChallengerCards.Select(x => x.Name));
        }

        [Theory]
        [InlineData(ChallengerId)]
        [InlineData(NonPlayerId)]
        [InlineData(null)]
        public void Should_not_return_host_cards(string playerId)
        {
            var game = CreateGame();

            var subject = new InProgress.GameStateDataStrategy();

            var actual = subject.GetData(game, Guid.TryParse(playerId, out var g) ? (Guid?)g : null);

            actual.Should().BeOfType(typeof(InProgress.Data));
            ((InProgress.Data)actual).HostCards
                .Should()
                .OnlyContain(x => x == "Hidden")
                .And
                .HaveCount(HostCards.Count());
        }

        [Theory]
        [InlineData(HostId)]
        [InlineData(NonPlayerId)]
        [InlineData(null)]
        public void Should_not_return_challenger_cards(string playerId)
        {
            var game = CreateGame();

            var subject = new InProgress.GameStateDataStrategy();

            var actual = subject.GetData(game, Guid.TryParse(playerId, out var g) ? (Guid?)g : null);

            actual.Should().BeOfType(typeof(InProgress.Data));
            ((InProgress.Data)actual).ChallengerCards
                .Should()
                .OnlyContain(x => x == "Hidden")
                .And
                .HaveCount(ChallengerCards.Count());
        }
    }
}