using System.Collections.Generic;
using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Enums;

namespace TripleTriad.SignalR
{
    public class GameDataMessage
    {
        public ICollection<string> Log { get; set; } = new List<string>();

        public bool PlayerOneTurn { get; set; }

        public IEnumerable<Tile> Tiles { get; set; }

        public Result? Result { get; set; }
    }
}