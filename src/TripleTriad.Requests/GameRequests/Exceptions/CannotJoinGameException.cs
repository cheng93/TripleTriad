using System;

namespace TripleTriad.Requests.GameRequests.Exceptions
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