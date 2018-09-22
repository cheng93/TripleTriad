using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Steps
{
    public class CoinTossStep : Step
    {
        private readonly string playerOneDisplay;
        private readonly string playerTwoDisplay;

        public CoinTossStep(
            GameData data,
            string playerOneDisplay,
            string playerTwoDisplay)
            : base(data)
        {
            this.playerOneDisplay = playerOneDisplay;
            this.playerTwoDisplay = playerTwoDisplay;
        }
    }
}