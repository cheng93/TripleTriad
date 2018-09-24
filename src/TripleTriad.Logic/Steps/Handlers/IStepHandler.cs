using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Steps.Handlers
{
    public interface IStepHandler<TStep>
        where TStep : Step
    {
        void ValidateAndThrow(TStep step);

        GameData Run(TStep step);
    }
}