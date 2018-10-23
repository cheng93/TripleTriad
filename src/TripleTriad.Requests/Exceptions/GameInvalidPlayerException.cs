using System;

namespace TripleTriad.Requests.Exceptions
{
    public class GameInvalidPlayerException : Exception
    {
        public GameInvalidPlayerException(int gameId, Guid playerId)
        {
            GameId = gameId;
            PlayerId = playerId;
        }

        public int GameId { get; }

        public Guid PlayerId { get; }
    }
}