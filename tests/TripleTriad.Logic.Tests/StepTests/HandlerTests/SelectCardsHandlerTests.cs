using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using TripleTriad.Logic.Cards;
using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Exceptions;
using TripleTriad.Logic.Steps;
using TripleTriad.Logic.Steps.Handlers;
using Xunit;

namespace TripleTriad.Logic.Tests.StepTests.HandlerTests
{
    public class SelectCardsHandlerTests
    {
        private static readonly IEnumerable<Card> Cards = new[]
        {
            AllCards.Squall,
            AllCards.Seifer,
            AllCards.Edea,
            AllCards.Rinoa,
            AllCards.Quistis
        };
        private static IEnumerable<string> CardNames
            = Cards.Select(x => x.Name);

        private static string PlayerDisplay = "PlayerDisplay";

        private static SelectCardsStep CreateStep(bool isHost = true, GameData gameData = null)
            => new SelectCardsStep(gameData ?? new GameData(), isHost, PlayerDisplay, CardNames);

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_have_correct_cards(bool isHost)
        {
            var subject = new SelectCardsHandler();
            var data = subject.Run(CreateStep(isHost));

            var cards = isHost
                ? data.HostCards
                : data.ChallengerCards;

            cards.Should().BeEquivalentTo(Cards);
        }

        [Fact]
        public void Should_have_correct_log_entry()
        {
            var subject = new SelectCardsHandler();
            var data = subject.Run(CreateStep());

            data.Log.Last().Should().Be($"{PlayerDisplay} has selected their cards.");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_throw_CardsAlreadySelectedException(bool isHost)
        {
            var gameData = new GameData();
            if (isHost)
            {
                gameData.HostCards = Cards;
            }
            else
            {
                gameData.ChallengerCards = Cards;
            }

            var subject = new SelectCardsHandler();
            Action act = () => subject.ValidateAndThrow(CreateStep(isHost, gameData));

            act.Should()
                .Throw<CardsAlreadySelectedException>()
                .Where(x => x.GameData == gameData
                    && x.IsHost == isHost);
        }
    }
}