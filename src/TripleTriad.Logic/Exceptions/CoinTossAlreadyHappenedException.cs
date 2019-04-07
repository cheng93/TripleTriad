using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Exceptions
{
    public class CoinTossAlreadyHappenedException : GameDataException
    {
        public CoinTossAlreadyHappenedException(GameData gameData, bool hostWonCoinToss)
            : base(gameData)
        {
            HostWonCoinToss = hostWonCoinToss;
        }

        public bool HostWonCoinToss { get; }
    }
}