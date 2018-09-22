using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using MediatR;
using TripleTriad.BackgroundTasks.Queue;
using TripleTriad.Logic.Cards;
using TripleTriad.Logic.Entities;
using TripleTriad.Requests.Pipeline;

namespace TripleTriad.Requests.GameRequests
{
    public static class CardSelect
    {
        public class Response
        {
            public int GameId { get; set; }

            public IEnumerable<Card> Cards { get; set; }
        }

        public class Request : IRequest<Response>
        {
            public int GameId { get; set; }

            public Guid PlayerId { get; set; }

            public IEnumerable<string> Cards { get; set; }
        }

        public class Validator : ValidationPreProcessor<Request>
        {
            public Validator()
            {
                base.RuleFor(x => x.GameId).NotEmpty();
                base.RuleFor(x => x.PlayerId).NotEmpty();
                base.RuleFor(x => x.Cards).NotNull();
                base.RuleFor(x => x.Cards.Count())
                    .Equal(5)
                    .When(x => x.Cards != null);
                base.RuleForEach(x => x.Cards)
                    .Must(x => AllCards.List.Any(y => y.Name == x))
                    .When(x => (x.Cards?.Count() ?? 0) == 5);
            }
        }

        public class GameStartBackgroundQueue : BackgroundQueuePostProcessor<Request, Response, GameStart.Request, GameStart.Response>
        {
            public GameStartBackgroundQueue(IBackgroundTaskQueue queue, IMediator mediator)
                : base(queue, mediator)
            {
            }

            protected override GameStart.Request CreateQueueRequest(Request request, Response response)
                => new GameStart.Request
                {
                    GameId = response.GameId
                };
        }
    }
}