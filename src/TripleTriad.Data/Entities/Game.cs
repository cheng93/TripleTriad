using System;
using TripleTriad.Data.Enums;

namespace TripleTriad.Data.Entities
{
    public class Game
    {
        public int GameId { get; set; }

        public Guid HostId { get; set; }

        public Guid? ChallengerId { get; set; }

        public string Data { get; set; }

        public GameStatus Status { get; set; }

        public virtual Player Host { get; set; }

        public virtual Player Challenger { get; set; }
    }
}