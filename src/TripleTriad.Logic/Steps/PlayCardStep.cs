using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Steps
{
    public class PlayCardStep : Step
    {
        public PlayCardStep(
            GameData data,
            string card,
            int tileId,
            bool isHost)
            : base(data)
        {
            Card = card;
            TileId = tileId;
            IsHost = isHost;
        }

        public string Card { get; }

        public int TileId { get; }

        public bool IsHost { get; }
    }
}