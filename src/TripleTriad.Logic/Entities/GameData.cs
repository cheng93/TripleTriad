﻿using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TripleTriad.Logic.Enums;

namespace TripleTriad.Logic.Entities
{
    public class GameData
    {
        public ICollection<string> Log { get; set; } = new List<string>();

        public IEnumerable<Rule> Rules { get; set; } = new List<Rule>();

        public bool? PlayerOneWonCoinToss { get; set; }

        public bool? PlayerOneTurn { get; set; }

        public IEnumerable<Card> PlayerOneCards { get; set; }

        public IEnumerable<Card> PlayerTwoCards { get; set; }

        public IEnumerable<Tile> Tiles { get; set; }

        public Result? Result { get; set; }
    }
}
