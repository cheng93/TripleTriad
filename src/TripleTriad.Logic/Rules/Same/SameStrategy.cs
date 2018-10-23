using System.Collections.Generic;
using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Rules.Same
{
    public class SameStrategy : IRuleStrategy
    {
        private readonly IRuleStrategy inner;
        private readonly bool wall;

        public SameStrategy(IRuleStrategy inner, bool wall)
        {
            this.inner = inner;
            this.wall = wall;
        }

        public IEnumerable<Tile> Apply(IEnumerable<Tile> tiles, int tileId)
        {
            // Apply same rule, otherwise continue.
            return this.inner.Apply(tiles, tileId);
        }
    }
}