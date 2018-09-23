using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Exceptions
{
    public class NotPlayerTurnException : GameDataException
    {
        public NotPlayerTurnException(GameData gameData, bool isPlayerOne)
            : base(gameData)
        {
            IsPlayerOne = isPlayerOne;
        }

        public bool IsPlayerOne { get; }
    }
}