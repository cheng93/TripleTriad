using TripleTriad.Logic.Enums;

namespace TripleTriad.Logic.Entities
{
    public class TileCard : Card
    {
        public TileCard(Card card, bool isPlayerOne)
            : base(card.Name, card.Level, card.Rank, card.Element)
        {
            IsPlayerOne = isPlayerOne;
        }

        public bool IsPlayerOne { get; }
    }
}