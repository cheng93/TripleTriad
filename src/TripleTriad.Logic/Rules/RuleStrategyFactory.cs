using System.Collections.Generic;
using System.Linq;
using TripleTriad.Logic.Enums;
using TripleTriad.Logic.Extensions;

namespace TripleTriad.Logic.Rules
{
    public class RuleStrategyFactory : IRuleStrategyFactory
    {
        public IRuleStrategy Create(IEnumerable<Rule> rules)
        {
            var builder = new RuleStrategyBuilder();

            rules = rules.ToList();
            if (rules.Contains(Rule.Same))
            {
                var wall = rules.Contains(Rule.Wall);
                builder = builder.AddSame(wall);
            }

            if (rules.Contains(Rule.Plus))
            {
                builder = builder.AddPlus();
            }

            if (rules.Contains(Rule.Elemental))
            {
                builder = builder.AddElemental();
            }

            return builder.Build();
        }
    }
}