using System;

namespace TripleTriad.Requests.GameRequests.Exceptions
{
    public class GameHasStartedException : Exception
    {
        public GameHasStartedException(int gameId)
        {
            GameId = gameId;
        }

        public int GameId { get; }
    }
}