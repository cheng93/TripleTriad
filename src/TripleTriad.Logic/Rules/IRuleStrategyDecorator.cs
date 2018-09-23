namespace TripleTriad.Logic.Rules
{
    internal interface IRuleStrategyDecorator
    {
        IRuleStrategy Decorate(IRuleStrategy strategy);
    }
}