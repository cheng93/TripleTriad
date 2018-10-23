using TripleTriad.Logic.Rules;
using TripleTriad.Logic.Rules.Elemental;
using TripleTriad.Logic.Rules.Plus;
using TripleTriad.Logic.Rules.Same;

namespace TripleTriad.Logic.Extensions
{
    internal static class RuleStrategyBuilderExtensions
    {
        public static RuleStrategyBuilder AddElemental(this RuleStrategyBuilder builder)
            => builder.Add(new ElementalDecorator());

        public static RuleStrategyBuilder AddSame(this RuleStrategyBuilder builder, bool wall)
            => builder.Add(new SameDecorator(wall));

        public static RuleStrategyBuilder AddPlus(this RuleStrategyBuilder builder)
            => builder.Add(new PlusDecorator());
    }
}