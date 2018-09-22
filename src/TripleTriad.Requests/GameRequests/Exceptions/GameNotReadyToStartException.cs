using System;

namespace TripleTriad.Requests.GameRequests.Exceptions
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