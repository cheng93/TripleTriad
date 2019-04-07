using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Exceptions
{
    public class NotPlayerTurnException : GameDataException
    {
        public NotPlayerTurnException(GameData gameData, bool isHost)
            : base(gameData)
        {
            IsHost = isHost;
        }

        public bool IsHost { get; }
    }
}