using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TripleTriad.Logic.Entities
{
    public class GameData
    {
        public ICollection<string> Log { get; set; } = new List<string>();

        public bool? PlayerOneWonCoinToss { get; set; }

        public bool? PlayerOneTurn { get; set; }

        public IEnumerable<string> PlayerOneCards { get; set; }

        public IEnumerable<string> PlayerTwoCards { get; set; }
    }
}
