using System.Collections.Generic;
using TripleTriad.Logic.Enums;

namespace TripleTriad.Logic.Rules
{
    public interface IRuleStrategyFactory
    {
        IRuleStrategy Create(IEnumerable<Rule> rules);
    }
}