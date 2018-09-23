using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using TripleTriad.Logic.Cards;
using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Enums;
using TripleTriad.Logic.Exceptions;
using TripleTriad.Logic.GameResult;
using TripleTriad.Logic.Steps;
using TripleTriad.Logic.Steps.Handlers;
using Xunit;

namespace TripleTriad.Logic.Tests.StepTests.HandlerTests
{
    public class PlayCardHandlerTests
    {
        private static readonly IEnumerable<Card> PlayerOneCards = new[]
        {
            AllCards.Laguna,
            AllCards.Kiros,
            AllCards.Ward,
            AllCards.Edea,
            AllCards.Seifer
        };

        private static readonly IEnumerable<Card> PlayerTwoCards = new[]
        {
            AllCards.Zell,
            AllCards.Quistis,
            AllCards.Selphie,
            AllCards.Irvine,
            AllCards.Seifer
        };

        public static Card Card = AllCards.Seifer;

        public static string MissingCardName = AllCards.Squall.Name;

        private static PlayCardStep CreateStep(GameData gameData, string cardName = null, int tileId = 0, bool isPlayerOne = true)
            => new PlayCardStep(
                    gameData,
                    cardName ?? AllCards.Seifer.Name,
                    tileId,
                    isPlayerOne);

        private static GameData CreateData() => new GameData
        {
            PlayerOneCards = PlayerOneCards,
            PlayerTwoCards = PlayerTwoCards
        };

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_throw_CardNotInHandException(bool isPlayerOne)
        {
            var gameData = CreateData();

            var gameResultService = new Mock<IGameResultService>();
            var subject = new PlayCardHandler(gameResultService.Object);

            Action act = () => subject.ValidateAndThrow(CreateStep(gameData, isPlayerOne: isPlayerOne, cardName: MissingCardName));

            act.Should()
                .Throw<CardNotInHandException>()
                .Where(x => x.GameData == gameData
                    && x.IsPlayerOne == isPlayerOne
                    && x.Card == MissingCardName);
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

            var gameData = CreateData();
            gameData.Tiles = Enumerable.Range(0, 9)
                .Select(x => new Tile
                {
                    TileId = x,
                    Card = x == tileId
                        ? new TileCard(AllCards.Squall, false)
                        : null
                });

            var gameResultService = new Mock<IGameResultService>();
            var subject = new PlayCardHandler(gameResultService.Object);

            Action act = () => subject.ValidateAndThrow(CreateStep(gameData, tileId: tileId));

            act.Should()
                .Throw<TileUnavailableException>()
                .Where(x => x.GameData == gameData
                    && x.TileId == tileId);
        }

        [Theory]
        [InlineData(Result.PlayerOneWin, "Player One has won.")]
        [InlineData(Result.PlayerTwoWin, "Player Two has won.")]
        [InlineData(Result.Tie, "Their was a tie.")]
        public void Should_have_correct_result_log_entry(Result result, string message)
        {
            var gameData = CreateData();
            var gameResultService = new Mock<IGameResultService>();
            gameResultService
                .Setup(x => x.GetResult(It.IsAny<GameData>()))
                .Returns(result);

            var subject = new PlayCardHandler(gameResultService.Object);

            var data = subject.Run(CreateStep(gameData));
            data.Log.Last().Should().Be(message);
        }

        [Theory]
        [InlineData(Result.PlayerOneWin)]
        [InlineData(Result.PlayerTwoWin)]
        [InlineData(Result.Tie)]
        [InlineData(null)]
        public void Should_have_correct_result(Result? result)
        {
            var gameData = CreateData();
            var gameResultService = new Mock<IGameResultService>();
            gameResultService
                .Setup(x => x.GetResult(It.IsAny<GameData>()))
                .Returns(result);

            var subject = new PlayCardHandler(gameResultService.Object);

            var data = subject.Run(CreateStep(gameData));
            data.Result.Should().Be(result);
        }

        public static IEnumerable<object[]> Moves()
        {
            for (var i = 0; i < 9; i++)
            {
                yield return new object[] { true, i, $"Player One played {Card.Name} on tile {i}" };
                yield return new object[] { false, i, $"Player Two played {Card.Name} on tile {i}" };
            }
        }

        [Theory]
        [MemberData(nameof(Moves))]
        public void Should_have_correct_move_log_entry(bool isPlayerOne, int tileId, string message)
        {
            var gameData = CreateData();
            var gameResultService = new Mock<IGameResultService>();

            var subject = new PlayCardHandler(gameResultService.Object);

            var data = subject.Run(CreateStep(gameData, isPlayerOne: isPlayerOne, tileId: tileId));
            data.Log.Should().Contain(message);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_have_remove_card_from_hand(bool isPlayerOne)
        {
            var gameData = CreateData();
            var gameResultService = new Mock<IGameResultService>();

            var subject = new PlayCardHandler(gameResultService.Object);

            var data = subject.Run(CreateStep(gameData, isPlayerOne: isPlayerOne));
            var cards = isPlayerOne ? data.PlayerOneCards : data.PlayerTwoCards;
            var expectedCards = (isPlayerOne ? PlayerOneCards : PlayerTwoCards)
                .Where(x => x.Name != Card.Name);

            cards.Should().BeEquivalentTo(expectedCards);
        }
    }
}