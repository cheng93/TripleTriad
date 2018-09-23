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
        private static readonly IEnumerable<Card> Cards = new[]
        {
            AllCards.Laguna,
            AllCards.Kiros,
            AllCards.Ward,
            AllCards.Edea,
            AllCards.Seifer
        };

        public static string CardName = AllCards.Squall.Name;

        private static PlayCardStep CreateStep(bool isPlayerOne = true, int tileId = 0, GameData gameData = null)
            => new PlayCardStep(gameData ?? new GameData(), CardName, tileId, isPlayerOne);

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

            var gameResultService = new Mock<IGameResultService>();
            var subject = new PlayCardHandler(gameResultService.Object);

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

            var gameResultService = new Mock<IGameResultService>();
            var subject = new PlayCardHandler(gameResultService.Object);

            Action act = () => subject.ValidateAndThrow(CreateStep(tileId, gameData));

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
            var gameData = new GameData();
            var gameResultService = new Mock<IGameResultService>();
            gameResultService
                .Setup(x => x.GetResult(It.IsAny<GameData>()))
                .Returns(result);

            var subject = new PlayCardHandler(gameResultService.Object);

            var data = subject.Run(CreateStep(true, 0, gameData: gameData));
            data.Log.Last().Should().Be(message);
        }

        [Theory]
        [InlineData(Result.PlayerOneWin)]
        [InlineData(Result.PlayerTwoWin)]
        [InlineData(Result.Tie)]
        [InlineData(null)]
        public void Should_have_correct_result(Result? result)
        {
            var gameData = new GameData();
            var gameResultService = new Mock<IGameResultService>();
            gameResultService
                .Setup(x => x.GetResult(It.IsAny<GameData>()))
                .Returns(result);

            var subject = new PlayCardHandler(gameResultService.Object);

            var data = subject.Run(CreateStep(true, gameData: gameData));
            data.Result.Should().Be(result);
        }

        public static IEnumerable<object[]> Moves()
        {
            for (var i = 0; i < 9; i++)
            {
                yield return new object[] { true, i, $"Player One played {CardName} on tile {i}" };
                yield return new object[] { false, i, $"Player Two played {CardName} on tile {i}" };
            }
        }

        [Theory]
        [MemberData(nameof(Moves))]
        public void Should_have_correct_move_log_entry(bool isPlayerOne, int tileId, string message)
        {
            var gameData = new GameData();
            var gameResultService = new Mock<IGameResultService>();

            var subject = new PlayCardHandler(gameResultService.Object);

            var data = subject.Run(CreateStep(isPlayerOne, tileId, gameData));
            data.Log.Should().Contain(message);
        }
    }
}