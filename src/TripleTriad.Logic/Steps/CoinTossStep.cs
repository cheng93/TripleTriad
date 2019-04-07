using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Steps
{
    public class CoinTossStep : Step
    {
        public CoinTossStep(
            GameData data,
            string hostDisplay,
            string playerTwoDisplay)
            : base(data)
        {
            HostDisplay = hostDisplay;
            PlayerTwoDisplay = playerTwoDisplay;
        }

        public string HostDisplay { get; }

        public string PlayerTwoDisplay { get; }
    }
}