namespace TripleTriad.Logic.ToinCoss
{
    public class ToinCossService : IToinCossService
    {
        private readonly IRandomWrapper random;

        public ToinCossService(IRandomWrapper random)
        {
            this.random = random;
        }

        public bool IsHeads() => this.random.Next(0, 100) < 50;
    }
}