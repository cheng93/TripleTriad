using System.Linq;
using TripleTriad.Logic.Cards;
using TripleTriad.Logic.Entities;
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

            if (step.IsPlayerOne)
            {
                step.Data.PlayerOneCards = cards;
            }
            else
            {
                step.Data.PlayerTwoCards = cards;
            }

            step.Log($"{step.PlayerDisplay} has selected their cards.");

            return step.Data;
        }
    }
}