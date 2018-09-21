using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TripleTriad.Logic.Entities
{
    public class GameData
    {
        public ICollection<string> Log { get; set; } = new List<string>();
    }
}
