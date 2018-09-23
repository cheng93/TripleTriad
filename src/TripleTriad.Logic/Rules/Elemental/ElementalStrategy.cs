using System.Collections.Generic;
using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Rules.Elemental
{
    public class ElementalStrategy : IRuleStrategy
    {
        private readonly IRuleStrategy inner;

        public ElementalStrategy(IRuleStrategy inner)
        {
            this.inner = inner;
        }

        public IEnumerable<Tile> Apply(IEnumerable<Tile> tiles, int tileId)
        {
            // Apply elemental values to tiles
            return this.inner.Apply(tiles, tileId);
        }
    }
}