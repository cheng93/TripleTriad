using System;
using TripleTriad.Data.Entities;

namespace TripleTriad.Requests.Messages.GameStateDataStrategies
{
    public interface IGameStateDataStrategy
    {
        GameStateData GetData(Game game, Guid? playerId);
    }
}