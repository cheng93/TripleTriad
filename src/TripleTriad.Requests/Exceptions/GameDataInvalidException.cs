using System;
using TripleTriad.Logic.Exceptions;

namespace TripleTriad.Requests.Exceptions
{
    public class GameDataInvalidException : Exception
    {
        public GameDataInvalidException(int gameId, GameDataException gameDataException)
            : base("Invalid Game Data", gameDataException)
        {
            GameId = gameId;
        }

        public int GameId { get; }
    }
}