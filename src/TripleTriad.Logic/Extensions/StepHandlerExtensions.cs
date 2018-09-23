using System.Collections.Generic;
using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Steps;
using TripleTriad.Logic.Steps.Handlers;

namespace TripleTriad.Logic.Extensions
{
    public static class StepHandlerExtensions
    {
        public static GameData ValidateAndRun<TStep>(
            this IStepHandler<TStep> handler,
            TStep step)
            where TStep : Step
        {
            handler.ValidateAndThrow(step);
            return handler.Run(step);
        }

        public static GameData Run(
            this IStepHandler<CoinTossStep> handler,
            GameData data,
            string playerOneDispay,
            string playerTwoDisplay)
        {
            var step = new CoinTossStep(data, playerOneDispay, playerTwoDisplay);
            return handler.ValidateAndRun(step);
        }

        public static GameData Run(
            this IStepHandler<SelectCardsStep> handler,
            GameData data,
            bool isPlayerOne,
            string playerDisplay,
            IEnumerable<string> cards)
        {
            var step = new SelectCardsStep(data, isPlayerOne, playerDisplay, cards);
            return handler.ValidateAndRun(step);
        }

        public static GameData Run(
            this IStepHandler<CreateBoardStep> handler,
            GameData data)
        {
            var step = new CreateBoardStep(data);
            return handler.ValidateAndRun(step);
        }
    }
}