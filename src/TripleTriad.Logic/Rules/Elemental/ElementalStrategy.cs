using System.Collections.Generic;
using System.Linq;
using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Extensions;

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
            tiles = tiles
                .Select(x =>
                {
                    if (x.TileId != tileId
                        || x.Element == null)
                    {
                        return x;
                    }

                    x.Card.Modifier = x.Element == x.Card.Element
                        ? 1
                        : -1;

                    return x;
                });

            return this.inner.Apply(tiles, tileId);
        }
    }
}