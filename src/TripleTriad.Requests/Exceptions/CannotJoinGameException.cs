using System;

namespace TripleTriad.Requests.Exceptions
{
    public class CannotJoinGameException : Exception
    {
        public CannotJoinGameException(int gameId)
        {
            GameId = gameId;
        }

        public int GameId { get; }
    }
}