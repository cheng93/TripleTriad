using System;

namespace TripleTriad.Requests.Exceptions
{
    public class PlayerStillSelectingCardsException : Exception
    {
        public PlayerStillSelectingCardsException(int gameId, bool playerOne, bool playerTwo)
        {
            GameId = gameId;
            PlayerOne = playerOne;
            PlayerTwo = playerTwo;
        }

        public int GameId { get; }

        public bool PlayerOne { get; }

        public bool PlayerTwo { get; }
    }
}