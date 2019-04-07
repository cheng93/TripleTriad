using System;
using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Exceptions
{
    public class PlayerStillSelectingCardsException : GameDataException
    {
        public PlayerStillSelectingCardsException(GameData gameData, bool host, bool challenger)
            : base(gameData)
        {
            Host = host;
            Challenger = challenger;
        }

        public bool Host { get; }

        public bool Challenger { get; }
    }
}