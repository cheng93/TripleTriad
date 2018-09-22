using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Steps
{
    public class CoinTossStep : Step
    {

        public CoinTossStep(
            GameData data,
            string playerOneDisplay,
            string playerTwoDisplay)
            : base(data)
        {
            PlayerOneDisplay = playerOneDisplay;
            PlayerTwoDisplay = playerTwoDisplay;
        }

        public string PlayerOneDisplay { get; }

        public string PlayerTwoDisplay { get; }
    }
}