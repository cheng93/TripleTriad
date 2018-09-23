using System.Linq;
using TripleTriad.Logic.CoinToss;
using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Exceptions;
using TripleTriad.Logic.Extensions;

namespace TripleTriad.Logic.Steps.Handlers
{
    public class CoinTossHandler : IStepHandler<CoinTossStep>
    {
        private readonly ICoinTossService coinTossService;

        public CoinTossHandler(ICoinTossService coinTossService)
        {
            this.coinTossService = coinTossService;
        }

        public GameData Run(CoinTossStep step)
        {
            var playerOneStarts = this.coinTossService.IsHeads();

            step.Data.PlayerOneTurn = playerOneStarts;
            step.Data.PlayerOneWonCoinToss = playerOneStarts;
            step.Log(LogTemplate(playerOneStarts ? step.PlayerOneDisplay : step.PlayerTwoDisplay));

            return step.Data;
        }

        private static string LogTemplate(string playerDisplay)
            => $"{playerDisplay} won the coin toss.";

        public void ValidateAndThrow(CoinTossStep step)
        {
            var playerOneNotSelectedCards = (step.Data.PlayerOneCards?.Count() ?? 0) != 5;
            var playerTwoNotSelectedCards = (step.Data.PlayerTwoCards?.Count() ?? 0) != 5;

            if (playerOneNotSelectedCards || playerTwoNotSelectedCards)
            {
                throw new PlayerStillSelectingCardsException(
                    step.Data,
                    playerOneNotSelectedCards,
                    playerTwoNotSelectedCards);
            }
        }
    }
}