using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Steps
{
    public class PlayCardStep : Step
    {
        public PlayCardStep(
            GameData data,
            string card,
            int tileId,
            bool isPlayerOne)
            : base(data)
        {
            Card = card;
            TileId = tileId;
            IsPlayerOne = isPlayerOne;
        }

        public string Card { get; }

        public int TileId { get; }

        public bool IsPlayerOne { get; }
    }
}