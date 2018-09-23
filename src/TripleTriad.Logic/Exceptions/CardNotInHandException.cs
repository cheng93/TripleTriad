using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Exceptions
{
    public class CardNotInHandException : GameDataException
    {
        public CardNotInHandException(
            GameData gameData,
            bool isPlayerOne,
            string card)
            : base(gameData)
        {
            IsPlayerOne = isPlayerOne;
            Card = card;
        }

        public bool IsPlayerOne { get; }

        public string Card { get; }
    }
}