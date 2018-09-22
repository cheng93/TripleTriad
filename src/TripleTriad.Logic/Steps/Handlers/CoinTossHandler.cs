using TripleTriad.Logic.CoinToss;
using TripleTriad.Logic.Entities;
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
    }
}