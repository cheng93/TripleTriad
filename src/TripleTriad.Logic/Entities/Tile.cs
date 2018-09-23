using TripleTriad.Logic.Enums;

namespace TripleTriad.Logic.Entities
{
    public class Tile
    {
        public int TileId { get; set; }

        public TileCard Card { get; set; }

        public Element? Element { get; set; }
    }
}