using System;

namespace TripleTriad.Requests.Exceptions
{
    public class GameNotReadyToStartException : Exception
    {
        public GameNotReadyToStartException(int gameId)
        {
            GameId = gameId;
        }

        public int GameId { get; }
    }
}