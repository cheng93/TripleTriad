using System.Linq;
using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Exceptions;

namespace TripleTriad.Logic.Steps.Handlers
{
    public class PlayCardHandler : IStepHandler<PlayCardStep>
    {
        public GameData Run(PlayCardStep step)
        {
            throw new System.NotImplementedException();
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