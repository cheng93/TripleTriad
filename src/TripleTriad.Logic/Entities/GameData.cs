using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TripleTriad.Logic.Enums;

namespace TripleTriad.Logic.Entities
{
    public class GameData
    {
        public ICollection<string> Log { get; set; } = new List<string>();

        public IEnumerable<Rule> Rules { get; set; } = new List<Rule>();

        public bool? HostWonCoinToss { get; set; }

        public bool? HostTurn { get; set; }

        public IEnumerable<Card> HostCards { get; set; }

        public IEnumerable<Card> ChallengerCards { get; set; }

        public IEnumerable<Tile> Tiles { get; set; }

        public Result? Result { get; set; }
    }
}
