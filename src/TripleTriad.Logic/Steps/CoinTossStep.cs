using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Steps
{
    public class CoinTossStep : Step
    {
        public CoinTossStep(
            GameData data,
            string hostDisplay,
            string challengerDisplay)
            : base(data)
        {
            HostDisplay = hostDisplay;
            ChallengerDisplay = challengerDisplay;
        }

        public string HostDisplay { get; }

        public string ChallengerDisplay { get; }
    }
}