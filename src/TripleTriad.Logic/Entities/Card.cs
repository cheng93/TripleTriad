using TripleTriad.Logic.Enums;

namespace TripleTriad.Logic.Entities
{
    public class Card
    {
        public Card(string name, int level, Rank rank, Element? element = null)
        {
            Name = name;
            Level = level;
            Rank = rank;
            Element = element;
        }

        public string Name { get; }

        public int Level { get; }

        public Rank Rank { get; }

        public Element? Element { get; }
    }
}