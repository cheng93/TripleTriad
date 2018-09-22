using System;

namespace TripleTriad.Requests.Exceptions
{
    public class CardsAlreadySelectedException : Exception
    {
        public CardsAlreadySelectedException(int gameId, Guid playerId)
        {
            GameId = gameId;
            PlayerId = playerId;
        }

        public int GameId { get; }

        public Guid PlayerId { get; }
    }
}