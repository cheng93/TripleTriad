using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Steps
{
    public abstract class Step
    {
        protected Step(GameData data)
        {
            this.Data = data;
        }

        protected GameData Data { get; }
    }
}