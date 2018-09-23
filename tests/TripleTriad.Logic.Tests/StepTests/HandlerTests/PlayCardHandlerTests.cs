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
    public class PlayCardHandlerTests
    {
        private static readonly IEnumerable<Card> Cards = new[]
        {
            AllCards.Laguna,
            AllCards.Kiros,
            AllCards.Ward,
            AllCards.Edea,
            AllCards.Seifer
        };

        public static string CardName = AllCards.Squall.Name;

        private static PlayCardStep CreateStep(bool isPlayerOne = true, GameData gameData = null)
            => new PlayCardStep(gameData ?? new GameData(), CardName, 0, isPlayerOne);

        private static PlayCardStep CreateStep(int tileId, GameData gameData = null)
            => new PlayCardStep(gameData ?? new GameData(), AllCards.Seifer.Name, tileId, true);

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_throw_CardsAlreadySelectedException(bool isPlayerOne)
        {
            var gameData = new GameData()
            {
                PlayerOneCards = Cards,
                PlayerTwoCards = Cards
            };

            var subject = new PlayCardHandler();
            Action act = () => subject.ValidateAndThrow(CreateStep(isPlayerOne, gameData));

            act.Should()
                .Throw<CardNotInHandException>()
                .Where(x => x.GameData == gameData
                    && x.IsPlayerOne == isPlayerOne
                    && x.Card == CardName);
        }

        public static IEnumerable<object[]> TileIds()
        {
            for (var i = 0; i < 9; i++)
            {
                yield return new object[] { i };
            }
        }

        [Theory]
        [MemberData(nameof(TileIds))]
        public void Should_throw_TileUnavailableException(int tileId)
        {
            var gameData = new GameData()
            {
                PlayerOneCards = Cards,
                PlayerTwoCards = Cards,
                Tiles = Enumerable.Range(0, 9)
                    .Select(x => new Tile
                    {
                        TileId = x,
                        Card = x == tileId
                            ? new TileCard(AllCards.Squall, false)
                            : null
                    })
            };

            var subject = new PlayCardHandler();
            Action act = () => subject.ValidateAndThrow(CreateStep(tileId, gameData));

            act.Should()
                .Throw<TileUnavailableException>()
                .Where(x => x.GameData == gameData
                    && x.TileId == tileId);
        }
    }
}