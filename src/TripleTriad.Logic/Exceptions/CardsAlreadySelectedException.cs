using System;
using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Exceptions
{
    public class CardsAlreadySelectedException : GameDataException
    {
        public CardsAlreadySelectedException(GameData gameData, bool isHost)
            : base(gameData)
        {
            IsHost = isHost;
        }

        public bool IsHost { get; }
    }
}