using System.Collections.Generic;
using System.Linq;
using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Enums;
using TripleTriad.Logic.Exceptions;
using TripleTriad.Logic.Extensions;
using TripleTriad.Logic.GameResult;

namespace TripleTriad.Logic.Steps.Handlers
{
    public class PlayCardHandler : IStepHandler<PlayCardStep>
    {
        private static Dictionary<Result, string> Messages
            = new Dictionary<Result, string>
            {
                {Result.PlayerOneWin, "Player One has won."},
                {Result.PlayerTwoWin, "Player Two has won."},
                {Result.Tie, "Their was a tie."}
            };

        private readonly IGameResultService gameResultService;

        public PlayCardHandler(IGameResultService gameResultService)
        {
            this.gameResultService = gameResultService;
        }

        public GameData Run(PlayCardStep step)
        {
            if (step.IsPlayerOne)
            {
                step.Data.PlayerOneCards = step.Data.PlayerOneCards
                    .Where(x => x.Name != step.Card);
            }
            else
            {
                step.Data.PlayerTwoCards = step.Data.PlayerTwoCards
                    .Where(x => x.Name != step.Card);
            }

            var player = $"Player {(step.IsPlayerOne ? "One" : "Two")}";
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
            var cards = step.IsPlayerOne ? step.Data.PlayerOneCards : step.Data.PlayerTwoCards;

            if (cards.All(x => x.Name != step.Card))
            {
                throw new CardNotInHandException(step.Data, step.IsPlayerOne, step.Card);
            }

            var tile = step.Data.Tiles.Single(x => x.TileId == step.TileId);
            if (tile.Card != null)
            {
                throw new TileUnavailableException(step.Data, step.TileId);
            }
        }
    }
}