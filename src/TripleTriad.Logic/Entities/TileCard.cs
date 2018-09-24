using Newtonsoft.Json;
using TripleTriad.Logic.Enums;

namespace TripleTriad.Logic.Entities
{
    public class TileCard : Card
    {
        public TileCard(Card card, bool isPlayerOne, int modifier = 0)
            : base(card.Name, card.Level, card.Rank, card.Element)
        {
            IsPlayerOne = isPlayerOne;
            Modifier = modifier;
        }

        [JsonConstructor]
        public TileCard(bool isPlayerOne, int modifier, string name, int level, Rank rank, Element? element = null)
            : this(new Card(name, level, rank, element), isPlayerOne, modifier)
        {
        }

        public bool IsPlayerOne { get; set; }

        public int Modifier { get; set; }
    }
}