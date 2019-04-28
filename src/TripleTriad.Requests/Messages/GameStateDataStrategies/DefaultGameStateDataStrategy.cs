using System;
using TripleTriad.Data.Entities;

namespace TripleTriad.Requests.Messages.GameStateDataStrategies
{
    public class DefaultGameStateDataStrategy : IGameStateDataStrategy
    {
        public GameStateData GetData(Game game, Guid? playerId)
            => new GameStateData
            {
                GameId = game.GameId,
                Status = game.Status.ToString()
            };
    }
}