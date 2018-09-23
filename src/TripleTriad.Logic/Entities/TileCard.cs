using Newtonsoft.Json;
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

        [JsonConstructor]
        public TileCard(bool isPlayerOne, string name, int level, Rank rank, Element? element = null)
            : this(new Card(name, level, rank, element), isPlayerOne)
        {
        }

        public bool IsPlayerOne { get; }
    }
}