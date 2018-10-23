using System;
using TripleTriad.Data.Enums;

namespace TripleTriad.Requests.Exceptions
{
    public class GameHasInvalidStatusException : Exception
    {
        public GameHasInvalidStatusException(int gameId, GameStatus status)
        {
            GameId = gameId;
            Status = status;
        }

        public int GameId { get; }

        public GameStatus Status { get; }
    }
}