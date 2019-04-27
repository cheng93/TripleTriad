using System;
using TripleTriad.Data.Entities;

namespace TripleTriad.Requests.Messages.GameStateStrategies
{
    public interface IGameStateDataStrategy
    {
        object GetData(Game game, Guid? playerId);
    }
}