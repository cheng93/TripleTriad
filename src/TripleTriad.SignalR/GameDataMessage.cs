using System.Collections.Generic;
using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Enums;

namespace TripleTriad.SignalR
{
    public class GameDataMessage
    {
        public int GameId { get; set; }

        public string Status { get; set; }

        public ICollection<string> Log { get; set; } = new List<string>();

        public bool? HostTurn { get; set; }

        public bool? HostWonCoinToss { get; set; }

        public IEnumerable<Tile> Tiles { get; set; }

        public Result? Result { get; set; }
    }
}