using System;

namespace TripleTriad.Requests.GameRequests.Exceptions
{
    public class GameNotFoundException : Exception
    {
        public GameNotFoundException(int gameId)
        {
            GameId = gameId;
        }

        public int GameId { get; }
    }
}