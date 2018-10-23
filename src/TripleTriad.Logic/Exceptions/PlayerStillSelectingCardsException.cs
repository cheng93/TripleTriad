using System;
using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Exceptions
{
    public class PlayerStillSelectingCardsException : GameDataException
    {
        public PlayerStillSelectingCardsException(GameData gameData, bool playerOne, bool playerTwo)
            : base(gameData)
        {
            PlayerOne = playerOne;
            PlayerTwo = playerTwo;
        }

        public bool PlayerOne { get; }

        public bool PlayerTwo { get; }
    }
}