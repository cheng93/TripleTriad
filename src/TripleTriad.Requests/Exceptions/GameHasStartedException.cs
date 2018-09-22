using System;

namespace TripleTriad.Requests.Exceptions
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