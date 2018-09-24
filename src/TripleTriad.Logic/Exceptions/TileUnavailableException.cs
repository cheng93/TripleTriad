using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Exceptions
{
    public class TileUnavailableException : GameDataException
    {
        public TileUnavailableException(GameData gameData, int tileId)
            : base(gameData)
        {
            TileId = tileId;
        }

        public int TileId { get; }
    }
}