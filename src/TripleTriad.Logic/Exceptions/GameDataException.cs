using System;
using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Exceptions
{
    public class GameDataException : Exception
    {
        public GameDataException(GameData gameData)
        {
            GameData = gameData;
        }

        public GameData GameData { get; }
    }
}