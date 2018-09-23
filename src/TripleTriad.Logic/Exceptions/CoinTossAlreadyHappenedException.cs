using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Exceptions
{
    public class CoinTossAlreadyHappenedException : GameDataException
    {
        public CoinTossAlreadyHappenedException(GameData gameData, bool playerOneWonCoinToss)
            : base(gameData)
        {
            PlayerOneWonCoinToss = playerOneWonCoinToss;
        }

        public bool PlayerOneWonCoinToss { get; }
    }
}