using TripleTriad.Data.Entities;

namespace TripleTriad.Requests.Messages.GameStateDataStrategies
{
    public interface IGameStateDataStrategyFactory
    {
        IGameStateDataStrategy GetStrategy(Game game);
    }
}