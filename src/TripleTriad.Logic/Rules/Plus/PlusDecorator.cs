namespace TripleTriad.Logic.Rules.Plus
{
    public class PlusDecorator : IRuleStrategyDecorator
    {
        public IRuleStrategy Decorate(IRuleStrategy strategy)
            => new PlusStrategy(strategy);
    }
}