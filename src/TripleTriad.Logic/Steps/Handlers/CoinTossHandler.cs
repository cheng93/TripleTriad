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
            var hostStarts = this.coinTossService.IsHeads();

            step.Data.HostTurn = hostStarts;
            step.Data.HostWonCoinToss = hostStarts;
            step.Log(LogTemplate(hostStarts ? step.HostDisplay : step.PlayerTwoDisplay));

            return step.Data;
        }

        private static string LogTemplate(string playerDisplay)
            => $"{playerDisplay} won the coin toss.";

        public void ValidateAndThrow(CoinTossStep step)
        {
            if (step.Data.HostWonCoinToss != null)
            {
                throw new CoinTossAlreadyHappenedException(step.Data, step.Data.HostWonCoinToss.Value);
            }

            var hostNotSelectedCards = (step.Data.HostCards?.Count() ?? 0) != 5;
            var playerTwoNotSelectedCards = (step.Data.PlayerTwoCards?.Count() ?? 0) != 5;

            if (hostNotSelectedCards || playerTwoNotSelectedCards)
            {
                throw new PlayerStillSelectingCardsException(
                    step.Data,
                    hostNotSelectedCards,
                    playerTwoNotSelectedCards);
            }
        }
    }
}