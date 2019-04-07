using System.Collections.Generic;
using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Steps
{
    public class SelectCardsStep : Step
    {
        public SelectCardsStep(
            GameData data,
            bool isHost,
            string playerDisplay,
            IEnumerable<string> cards)
            : base(data)
        {
            IsHost = isHost;
            PlayerDisplay = playerDisplay;
            Cards = cards;
        }

        public bool IsHost { get; }

        public string PlayerDisplay { get; }

        public IEnumerable<string> Cards { get; }
    }
}