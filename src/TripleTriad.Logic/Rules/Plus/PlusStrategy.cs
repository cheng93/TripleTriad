using System.Collections.Generic;
using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Rules.Plus
{
    public class PlusStrategy : IRuleStrategy
    {
        private readonly IRuleStrategy inner;

        public PlusStrategy(IRuleStrategy inner)
        {
            this.inner = inner;
        }

        public IEnumerable<Tile> Apply(IEnumerable<Tile> tiles, int tileId)
        {
            // Apply plus rule, if any tiles captured, return, otherwise continue
            return this.inner.Apply(tiles, tileId);
        }
    }
}