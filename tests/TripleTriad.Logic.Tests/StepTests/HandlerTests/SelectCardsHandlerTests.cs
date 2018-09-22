using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using TripleTriad.Logic.Cards;
using TripleTriad.Logic.Entities;
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

        private static SelectCardsStep CreateStep(bool isPlayerOne = true)
            => new SelectCardsStep(new GameData(), isPlayerOne, PlayerDisplay, CardNames);

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_have_correct_cards(bool isPlayerOne)
        {
            var subject = new SelectCardsHandler();
            var data = subject.Run(CreateStep(isPlayerOne));

            var cards = isPlayerOne
                ? data.PlayerOneCards
                : data.PlayerTwoCards;

            cards.Should().BeEquivalentTo(Cards);
        }

        [Fact]
        public void Should_have_correct_log_entry()
        {
            var subject = new SelectCardsHandler();
            var data = subject.Run(CreateStep());

            data.Log.Last().Should().Be($"{PlayerDisplay} has selected their cards.");
        }
    }
}