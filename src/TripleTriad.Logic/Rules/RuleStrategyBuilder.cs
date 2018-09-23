namespace TripleTriad.Logic.Rules
{
    internal class RuleStrategyBuilder
    {
        private IRuleStrategy strategy = new DefaultRuleStrategy();

        public RuleStrategyBuilder Add(IRuleStrategyDecorator strategy)
        {
            this.strategy = strategy.Decorate(this.strategy);
            return this;
        }

        public IRuleStrategy Build() => this.strategy;
    }
}