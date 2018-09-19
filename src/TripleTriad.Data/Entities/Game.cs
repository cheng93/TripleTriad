using System;

namespace TripleTriad.Data.Entities
{
    public class Game
    {
        public int GameId { get; set; }

        public Guid PlayerOneId { get; set; }

        public Guid PlayerTwoId { get; set; }

        public virtual Player PlayerOne { get; set; }

        public virtual Player PlayerTwo { get; set; }
    }
}