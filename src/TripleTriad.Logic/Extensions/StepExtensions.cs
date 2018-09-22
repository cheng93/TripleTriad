using TripleTriad.Logic.Steps;

namespace TripleTriad.Logic.Extensions
{
    public static class StepExtensions
    {
        public static void Log(this Step step, string message) => step.Data.Log.Add(message);
    }
}