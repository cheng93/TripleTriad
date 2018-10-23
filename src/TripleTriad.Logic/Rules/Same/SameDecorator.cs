namespace TripleTriad.Logic.Rules.Same
{
    internal class SameDecorator : IRuleStrategyDecorator
    {
        private readonly bool wall;

        public SameDecorator(bool wall)
        {
            this.wall = wall;
        }

        public IRuleStrategy Decorate(IRuleStrategy strategy)
            => new SameStrategy(strategy, this.wall);
    }
}