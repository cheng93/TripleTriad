using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Steps.Handlers
{
    public interface IStepHandler<TStep>
        where TStep : Step
    {
        GameData Run(TStep step);
    }
}