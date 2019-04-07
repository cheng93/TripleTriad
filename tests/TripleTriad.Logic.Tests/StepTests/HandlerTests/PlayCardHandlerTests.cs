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
using TripleTriad.Logic.Rules;
using TripleTriad.Logic.Steps;
using TripleTriad.Logic.Steps.Handlers;
using Xunit;

namespace TripleTriad.Logic.Tests.StepTests.HandlerTests
{
    public class PlayCardHandlerTests
    {
        private static readonly IEnumerable<Card> HostCards = new[]
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

        private static PlayCardStep CreateStep(GameData gameData, string cardName = null, int tileId = 0, bool isHost = true)
            => new PlayCardStep(
                    gameData,
                    cardName ?? AllCards.Seifer.Name,
                    tileId,
                    isHost);

        private static GameData CreateData(bool isHost = true) => new GameData
        {
            HostTurn = isHost,
            HostCards = HostCards,
            PlayerTwoCards = PlayerTwoCards,
            Tiles = Enumerable.Range(0, 9)
                .Select(x => new Tile
                {
                    TileId = x
                })
        };

        private static IRuleStrategy CreateRuleStrategy()
        {
            var mock = new Mock<IRuleStrategy>();
            mock
                .Setup(x => x.Apply(
                    It.IsAny<IEnumerable<Tile>>(),
                    It.IsAny<int>()))
                .Returns<IEnumerable<Tile>, int>((x, y) => x);

            return mock.Object;
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_throw_NotPlayerTurnException(bool isHost)
        {
            var gameData = CreateData(!isHost);

            var gameResultService = new Mock<IGameResultService>();
            var ruleStrategyFactory = new Mock<IRuleStrategyFactory>();
            var subject = new PlayCardHandler(
                gameResultService.Object,
                ruleStrategyFactory.Object);

            Action act = () => subject.ValidateAndThrow(CreateStep(gameData, isHost: isHost, cardName: MissingCardName));

            act.Should()
                .Throw<NotPlayerTurnException>()
                .Where(x => x.GameData == gameData
                    && x.IsHost == isHost);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_throw_CardNotInHandException(bool isHost)
        {
            var gameData = CreateData(isHost);

            var gameResultService = new Mock<IGameResultService>();
            var ruleStrategyFactory = new Mock<IRuleStrategyFactory>();
            var subject = new PlayCardHandler(
                gameResultService.Object,
                ruleStrategyFactory.Object);

            Action act = () => subject.ValidateAndThrow(CreateStep(gameData, isHost: isHost, cardName: MissingCardName));

            act.Should()
                .Throw<CardNotInHandException>()
                .Where(x => x.GameData == gameData
                    && x.IsHost == isHost
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
            var ruleStrategyFactory = new Mock<IRuleStrategyFactory>();
            var subject = new PlayCardHandler(
                gameResultService.Object,
                ruleStrategyFactory.Object);

            Action act = () => subject.ValidateAndThrow(CreateStep(gameData, tileId: tileId));

            act.Should()
                .Throw<TileUnavailableException>()
                .Where(x => x.GameData == gameData
                    && x.TileId == tileId);
        }

        [Theory]
        [InlineData(Result.HostWin, "Player One has won.")]
        [InlineData(Result.PlayerTwoWin, "Player Two has won.")]
        [InlineData(Result.Tie, "There was a tie.")]
        public void Should_have_correct_result_log_entry(Result result, string message)
        {
            var gameData = CreateData();

            var gameResultService = new Mock<IGameResultService>();
            gameResultService
                .Setup(x => x.GetResult(It.IsAny<GameData>()))
                .Returns(result);

            var ruleStrategyFactory = new Mock<IRuleStrategyFactory>();
            ruleStrategyFactory
                .Setup(x => x.Create(It.IsAny<IEnumerable<Rule>>()))
                .Returns(CreateRuleStrategy());

            var subject = new PlayCardHandler(
                gameResultService.Object,
                ruleStrategyFactory.Object);

            var data = subject.Run(CreateStep(gameData));
            data.Log.Last().Should().Be(message);
        }

        [Theory]
        [InlineData(Result.HostWin)]
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

            var ruleStrategyFactory = new Mock<IRuleStrategyFactory>();
            ruleStrategyFactory
                .Setup(x => x.Create(It.IsAny<IEnumerable<Rule>>()))
                .Returns(CreateRuleStrategy());

            var subject = new PlayCardHandler(
                gameResultService.Object,
                ruleStrategyFactory.Object);

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
        public void Should_have_correct_move_log_entry(bool isHost, int tileId, string message)
        {
            var gameData = CreateData(isHost);

            var gameResultService = new Mock<IGameResultService>();

            var ruleStrategyFactory = new Mock<IRuleStrategyFactory>();
            ruleStrategyFactory
                .Setup(x => x.Create(It.IsAny<IEnumerable<Rule>>()))
                .Returns(CreateRuleStrategy());

            var subject = new PlayCardHandler(
                gameResultService.Object,
                ruleStrategyFactory.Object);

            var data = subject.Run(CreateStep(gameData, isHost: isHost, tileId: tileId));
            data.Log.Should().Contain(message);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_have_remove_card_from_hand(bool isHost)
        {
            var gameData = CreateData(isHost);

            var gameResultService = new Mock<IGameResultService>();

            var ruleStrategyFactory = new Mock<IRuleStrategyFactory>();
            ruleStrategyFactory
                .Setup(x => x.Create(It.IsAny<IEnumerable<Rule>>()))
                .Returns(CreateRuleStrategy());

            var subject = new PlayCardHandler(
                gameResultService.Object,
                ruleStrategyFactory.Object);

            var data = subject.Run(CreateStep(gameData, isHost: isHost));
            var cards = isHost ? data.HostCards : data.PlayerTwoCards;
            var expectedCards = (isHost ? HostCards : PlayerTwoCards)
                .Where(x => x.Name != Card.Name);

            cards.Should().BeEquivalentTo(expectedCards);
        }

        public static IEnumerable<object[]> AssignCard()
        {
            for (var i = 0; i < 9; i++)
            {
                yield return new object[] { true, i };
                yield return new object[] { false, i };
            }
        }

        [Theory]
        [MemberData(nameof(TileIds))]
        public void Should_assign_card_to_tile(int tileId)
        {
            var gameData = CreateData();

            var gameResultService = new Mock<IGameResultService>();

            var ruleStrategyFactory = new Mock<IRuleStrategyFactory>();
            ruleStrategyFactory
                .Setup(x => x.Create(It.IsAny<IEnumerable<Rule>>()))
                .Returns(CreateRuleStrategy());

            var subject = new PlayCardHandler(
                gameResultService.Object,
                ruleStrategyFactory.Object);

            var data = subject.Run(CreateStep(gameData, tileId: tileId));
            var tile = data.Tiles.Single(x => x.TileId == tileId);

            tile.Card.Should().BeEquivalentTo(Card, options => options.ExcludingMissingMembers());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_have_player_assigned_to_card(bool isHost)
        {
            var gameData = CreateData(isHost);

            var gameResultService = new Mock<IGameResultService>();

            var ruleStrategyFactory = new Mock<IRuleStrategyFactory>();
            ruleStrategyFactory
                .Setup(x => x.Create(It.IsAny<IEnumerable<Rule>>()))
                .Returns(CreateRuleStrategy());

            var subject = new PlayCardHandler(
                gameResultService.Object,
                ruleStrategyFactory.Object);

            var data = subject.Run(CreateStep(gameData, isHost: isHost));
            var tile = data.Tiles.Single(x => x.TileId == 0);

            tile.Card.IsHost.Should().Be(isHost);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_switch_turn(bool isHost)
        {
            var gameData = CreateData(isHost);

            var gameResultService = new Mock<IGameResultService>();

            var ruleStrategyFactory = new Mock<IRuleStrategyFactory>();
            ruleStrategyFactory
                .Setup(x => x.Create(It.IsAny<IEnumerable<Rule>>()))
                .Returns(CreateRuleStrategy());

            var subject = new PlayCardHandler(
                gameResultService.Object,
                ruleStrategyFactory.Object);

            var data = subject.Run(CreateStep(gameData, isHost: isHost));

            data.HostTurn.Should().Be(!isHost);
        }

        [Fact]
        public void Should_update_tiles_after_processing_rules()
        {
            var gameData = CreateData();

            var gameResultService = new Mock<IGameResultService>();

            var tiles = Enumerable.Range(0, 9)
                .Select(x => new Tile
                {
                    TileId = x,
                    Element = Element.Fire
                });

            var ruleStrategy = new Mock<IRuleStrategy>();
            ruleStrategy
                .Setup(x => x.Apply(
                    It.IsAny<IEnumerable<Tile>>(),
                    It.IsAny<int>()))
                .Returns(tiles);

            var ruleStrategyFactory = new Mock<IRuleStrategyFactory>();
            ruleStrategyFactory
                .Setup(x => x.Create(It.IsAny<IEnumerable<Rule>>()))
                .Returns(ruleStrategy.Object);

            var subject = new PlayCardHandler(
                gameResultService.Object,
                ruleStrategyFactory.Object);

            var data = subject.Run(CreateStep(gameData));

            data.Tiles.Should().BeEquivalentTo(tiles);
        }
    }
}