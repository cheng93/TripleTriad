using System;

namespace TripleTriad.Requests.Exceptions
{
    public class CannotPlayYourselfException : Exception
    {
        public CannotPlayYourselfException(int gameId, Guid playerId)
        {
            GameId = gameId;
            PlayerId = playerId;
        }

        public int GameId { get; }

        public Guid PlayerId { get; }
    }
}