using Newtonsoft.Json;
using TripleTriad.Logic.Enums;

namespace TripleTriad.Logic.Entities
{
    public class TileCard : Card
    {
        public TileCard(Card card, bool isHost, int modifier = 0)
            : base(card.Name, card.Level, card.Rank, card.Element)
        {
            IsHost = isHost;
            Modifier = modifier;
        }

        [JsonConstructor]
        public TileCard(bool isHost, int modifier, string name, int level, Rank rank, Element? element = null)
            : this(new Card(name, level, rank, element), isHost, modifier)
        {
        }

        public bool IsHost { get; set; }

        public int Modifier { get; set; }
    }
}