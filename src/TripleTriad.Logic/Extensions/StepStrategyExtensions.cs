using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Steps;
using TripleTriad.Logic.Steps.Strategies;

namespace TripleTriad.Logic.Extensions
{
    public static class StepStrategyExtensions
    {
        public static GameData Run(
            this IStepStrategy<CoinTossStep> strategy,
            GameData data,
            string playerOneDispay,
            string playerTwoDisplay)
        {
            var step = new CoinTossStep(data, playerOneDispay, playerTwoDisplay);
            return strategy.Run(step);
        }
    }
}