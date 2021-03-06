using System;
using TripleTriad.Data.Entities;
using TripleTriad.Data.Enums;

namespace TripleTriad.Requests.Messages.GameStateDataStrategies
{
    public class GameStateDataStrategyFactory : IGameStateDataStrategyFactory
    {
        public IGameStateDataStrategy GetStrategy(Game game)
        {
            switch (game.Status)
            {
                case GameStatus.ChooseCards:
                    return new ChooseCards.GameStateDataStrategy();
                case GameStatus.InProgress:
                    return new InProgress.GameStateDataStrategy();
                default:
                    return new DefaultGameStateDataStrategy();
            }
        }
    }
}