using System.Collections.Generic;

namespace TripleTriad.Logic.Entities
{
    public class GameData
    {
        public GameStatus Status { get; set; } = GameStatus.Waiting;

        public ICollection<string> Log { get; set; } = new List<string>();
    }
}
