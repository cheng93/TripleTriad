using System;
using System.Collections.Generic;
using System.Linq;
using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Capture
{
    internal class CaptureService : ICaptureService
    {
        private static Dictionary<int, Dictionary<int, Func<Rank, int>>> Neighbours
            = new Dictionary<int, Dictionary<int, Func<Rank, int>>>()
            {
                {0, new Dictionary<int, Func<Rank, int>>() { { 1, (r) => r.Right }, { 3, (r) => r.Bottom } }},
                {1, new Dictionary<int, Func<Rank, int>>() { { 0, (r) => r.Left }, { 2, (r) => r.Right }, { 4, (r) => r.Top } }},
                {2, new Dictionary<int, Func<Rank, int>>() { { 1, (r) => r.Left }, { 5, (r) => r.Bottom } }},
                {3, new Dictionary<int, Func<Rank, int>>() { { 0, (r) => r.Top }, { 4, (r) => r.Right }, { 6, (r) => r.Bottom } }},
                {4, new Dictionary<int, Func<Rank, int>>() { { 1, (r) => r.Top }, { 3, (r) => r.Left }, { 5, (r) => r.Right }, { 7, (r) => r.Bottom } }},
                {5, new Dictionary<int, Func<Rank, int>>() { { 2, (r) => r.Top }, { 4, (r) => r.Left }, { 8, (r) => r.Bottom } }},
                {6, new Dictionary<int, Func<Rank, int>>() { { 3, (r) => r.Top }, { 7, (r) => r.Right } }},
                {7, new Dictionary<int, Func<Rank, int>>() { { 4, (r) => r.Top }, { 6, (r) => r.Left }, { 8, (r) => r.Right } }},
                {8, new Dictionary<int, Func<Rank, int>>() { { 5, (r) => r.Top }, { 7, (r) => r.Left } }},
            };

        public IEnumerable<int> Captures(IEnumerable<Tile> tiles, int tileId)
        {
            var tile = tiles.Single(x => x.TileId == tileId);

            foreach (var neighbourTileId in Neighbours[tileId].Keys)
            {
                var neighbourTile = tiles.Single(x => x.TileId == neighbourTileId);
                if (neighbourTile.Card != null
                    && neighbourTile.Card.IsPlayerOne != tile.Card.IsPlayerOne)
                {
                    var tileTotalValue = Neighbours[tileId][neighbourTileId](tile.Card.Rank) + tile.Card.Modifier;
                    var neighbourTotalValue = Neighbours[neighbourTileId][tileId](neighbourTile.Card.Rank) + neighbourTile.Card.Modifier;

                    if (tileTotalValue > neighbourTotalValue)
                    {
                        yield return neighbourTileId;
                    }
                }
            }
        }
    }
}