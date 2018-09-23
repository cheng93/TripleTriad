using System;
using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Exceptions
{
    public class CardsAlreadySelectedException : GameDataException
    {
        public CardsAlreadySelectedException(GameData gameData, bool isPlayerOne)
            : base(gameData)
        {
            IsPlayerOne = isPlayerOne;
        }

        public bool IsPlayerOne { get; }
    }
}