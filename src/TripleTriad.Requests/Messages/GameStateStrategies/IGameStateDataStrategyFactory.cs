using TripleTriad.Data.Entities;

namespace TripleTriad.Requests.Messages.GameStateStrategies
{
    public interface IGameStateDataStrategyFactory
    {
        IGameStateDataStrategy GetStrategy(Game game);
    }
}