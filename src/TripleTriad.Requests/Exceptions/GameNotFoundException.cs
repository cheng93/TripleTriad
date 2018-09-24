using System;

namespace TripleTriad.Requests.Exceptions
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