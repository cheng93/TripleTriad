using System.Collections.Generic;
using System.Linq;
using TripleTriad.Logic.Capture;
using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Rules
{
    public class DefaultRuleStrategy : IRuleStrategy
    {
        private readonly ICaptureService captureService;

        public DefaultRuleStrategy(ICaptureService captureService)
        {
            this.captureService = captureService;
        }

        public IEnumerable<Tile> Apply(IEnumerable<Tile> tiles, int tileId)
        {
            var captured = this.captureService
                .Captures(tiles, tileId)
                .ToList();

            var tile = tiles.Single(x => x.TileId == tileId);

            return tiles
                .Select(x =>
                {
                    if (captured.Contains(x.TileId))
                    {
                        x.Card.IsHost = tile.Card.IsHost;
                    }
                    return x;
                });
        }
    }
}