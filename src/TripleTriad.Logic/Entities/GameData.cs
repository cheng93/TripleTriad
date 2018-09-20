using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TripleTriad.Logic.Entities
{
    public class GameData
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public GameStatus Status { get; set; } = GameStatus.Waiting;

        public ICollection<string> Log { get; set; } = new List<string>();
    }
}
