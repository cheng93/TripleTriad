using System.Collections.Generic;
using System.Linq;
using TripleTriad.Logic.Cards;
using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Enums;
using TripleTriad.Logic.Exceptions;
using TripleTriad.Logic.Extensions;
using TripleTriad.Logic.GameResult;
using TripleTriad.Logic.Rules;

namespace TripleTriad.Logic.Steps.Handlers
{
    public class PlayCardHandler : IStepHandler<PlayCardStep>
    {
        private static Dictionary<Result, string> Messages
            = new Dictionary<Result, string>
            {
                {Result.HostWin, "Player One has won."},
                {Result.PlayerTwoWin, "Player Two has won."},
                {Result.Tie, "There was a tie."}
            };

        private readonly IGameResultService gameResultService;
        private readonly IRuleStrategyFactory ruleStrategyFactory;

        public PlayCardHandler(
            IGameResultService gameResultService,
            IRuleStrategyFactory ruleStrategyFactory)
        {
            this.gameResultService = gameResultService;
            this.ruleStrategyFactory = ruleStrategyFactory;
        }

        public GameData Run(PlayCardStep step)
        {
            var card = AllCards.List.Single(x => x.Name == step.Card);

            step.Data.Tiles = step.Data.Tiles
                .Select(x =>
                {
                    if (x.TileId == step.TileId)
                    {
                        x.Card = new TileCard(card, step.IsHost);
                    }
                    return x;
                });

            step.Data.Tiles = this.ruleStrategyFactory
                .Create(step.Data.Rules)
                .Apply(step.Data.Tiles, step.TileId);

            if (step.IsHost)
            {
                step.Data.HostCards = step.Data.HostCards
                    .Where(x => x.Name != step.Card);
            }
            else
            {
                step.Data.PlayerTwoCards = step.Data.PlayerTwoCards
                    .Where(x => x.Name != step.Card);
            }

            var player = $"Player {(step.IsHost ? "One" : "Two")}";
            step.Data.HostTurn = !step.Data.HostTurn;
            step.Log($"{player} played {step.Card} on tile {step.TileId}");

            var result = this.gameResultService.GetResult(step.Data);
            if (result.HasValue)
            {
                step.Data.Result = result;
                step.Log(Messages[result.Value]);
            }

            return step.Data;
        }

        public void ValidateAndThrow(PlayCardStep step)
        {
            if (step.IsHost != step.Data.HostTurn)
            {
                throw new NotPlayerTurnException(step.Data, step.IsHost);
            }

            var cards = step.IsHost ? step.Data.HostCards : step.Data.PlayerTwoCards;

            if (cards.All(x => x.Name != step.Card))
            {
                throw new CardNotInHandException(step.Data, step.IsHost, step.Card);
            }

            var tile = step.Data.Tiles.Single(x => x.TileId == step.TileId);
            if (tile.Card != null)
            {
                throw new TileUnavailableException(step.Data, step.TileId);
            }
        }
    }
}