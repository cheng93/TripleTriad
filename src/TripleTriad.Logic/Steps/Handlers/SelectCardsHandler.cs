using System.Linq;
using TripleTriad.Logic.Cards;
using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Exceptions;
using TripleTriad.Logic.Extensions;

namespace TripleTriad.Logic.Steps.Handlers
{
    public class SelectCardsHandler : IStepHandler<SelectCardsStep>
    {
        public GameData Run(SelectCardsStep step)
        {
            var cards = AllCards.List
                .Where(x => step.Cards.Any(y => y == x.Name))
                .ToList();

            if (step.IsHost)
            {
                step.Data.HostCards = cards;
            }
            else
            {
                step.Data.ChallengerCards = cards;
            }

            step.Log($"{step.PlayerDisplay} has selected their cards.");

            return step.Data;
        }

        public void ValidateAndThrow(SelectCardsStep step)
        {
            var cards = step.IsHost ? step.Data.HostCards : step.Data.ChallengerCards;

            if ((cards?.Count() ?? 0) == 5)
            {
                throw new CardsAlreadySelectedException(step.Data, step.IsHost);
            }
        }
    }
}