using System.Collections.Generic;
using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Steps
{
    public class SelectCardsStep : Step
    {
        public SelectCardsStep(
            GameData data,
            bool isPlayerOne,
            string playerDisplay,
            IEnumerable<string> cards)
            : base(data)
        {
            IsPlayerOne = isPlayerOne;
            PlayerDisplay = playerDisplay;
            Cards = cards;
        }

        public bool IsPlayerOne { get; }

        public string PlayerDisplay { get; }

        public IEnumerable<string> Cards { get; }
    }
}