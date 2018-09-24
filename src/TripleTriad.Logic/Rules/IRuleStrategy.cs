using System.Collections.Generic;
using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Rules
{
    public interface IRuleStrategy
    {
        IEnumerable<Tile> Apply(IEnumerable<Tile> tiles, int tileId);
    }
}