using System;
using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Exceptions
{
    public class PlayerStillSelectingCardsException : GameDataException
    {
        public PlayerStillSelectingCardsException(GameData gameData, bool host, bool playerTwo)
            : base(gameData)
        {
            Host = host;
            PlayerTwo = playerTwo;
        }

        public bool Host { get; }

        public bool PlayerTwo { get; }
    }
}