using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Steps.Strategies
{
    public interface IStepStrategy<TStep>
        where TStep : Step
    {
        GameData Run(TStep step);
    }
}