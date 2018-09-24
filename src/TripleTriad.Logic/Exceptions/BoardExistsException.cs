using System;
using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Exceptions
{
    public class BoardExistsException : GameDataException
    {
        public BoardExistsException(GameData gameData)
            : base(gameData)
        {
        }
    }
}