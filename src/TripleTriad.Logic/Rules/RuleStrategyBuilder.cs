using TripleTriad.Logic.Capture;

namespace TripleTriad.Logic.Rules
{
    internal class RuleStrategyBuilder
    {
        private IRuleStrategy strategy = new DefaultRuleStrategy(new CaptureService());

        public RuleStrategyBuilder Add(IRuleStrategyDecorator strategy)
        {
            this.strategy = strategy.Decorate(this.strategy);
            return this;
        }

        public IRuleStrategy Build() => this.strategy;
    }
}