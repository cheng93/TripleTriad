using System;

namespace TripleTriad.Data.Entities
{
    public class Player
    {
        public Guid PlayerId { get; set; }

        public string DisplayName { get; set; }

        public int? AccountId { get; set; }
    }
}