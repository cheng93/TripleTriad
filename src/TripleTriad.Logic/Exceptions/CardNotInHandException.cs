using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Exceptions
{
    public class CardNotInHandException : GameDataException
    {
        public CardNotInHandException(
            GameData gameData,
            bool isHost,
            string card)
            : base(gameData)
        {
            IsHost = isHost;
            Card = card;
        }

        public bool IsHost { get; }

        public string Card { get; }
    }
}