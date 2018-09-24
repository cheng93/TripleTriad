using System.Collections.Generic;
using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Capture
{
    public interface ICaptureService
    {
        IEnumerable<int> Captures(IEnumerable<Tile> tiles, int tileId);
    }
}