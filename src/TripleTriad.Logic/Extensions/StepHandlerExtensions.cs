using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Steps;
using TripleTriad.Logic.Steps.Handlers;

namespace TripleTriad.Logic.Extensions
{
    public static class StepHandlerExtensions
    {
        public static GameData Run(
            this IStepHandler<CoinTossStep> strategy,
            GameData data,
            string playerOneDispay,
            string playerTwoDisplay)
        {
            var step = new CoinTossStep(data, playerOneDispay, playerTwoDisplay);
            return strategy.Run(step);
        }
    }
}