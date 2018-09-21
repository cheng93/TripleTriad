namespace TripleTriad.Logic.CoinToss
{
    public class CoinTossService : ICoinTossService
    {
        private readonly IRandomWrapper random;

        public CoinTossService(IRandomWrapper random)
        {
            this.random = random;
        }

        public bool IsHeads() => this.random.Next(0, 100) < 50;
    }
}