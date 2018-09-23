namespace TripleTriad.Logic.Rules.Elemental
{
    internal class ElementalDecorator : IRuleStrategyDecorator
    {
        public IRuleStrategy Decorate(IRuleStrategy strategy)
            => new ElementalStrategy(strategy);
    }
}