using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;
using TripleTriad.BackgroundTasks.Queue;
using TripleTriad.Data;
using TripleTriad.Data.Enums;
using TripleTriad.Logic.Cards;
using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Exceptions;
using TripleTriad.Logic.Extensions;
using TripleTriad.Logic.Steps;
using TripleTriad.Logic.Steps.Handlers;
using TripleTriad.Requests.Exceptions;
using TripleTriad.Requests.Extensions;
using TripleTriad.Requests.Pipeline;
using TripleTriad.Requests.Response;

namespace TripleTriad.Requests.GameRequests
{
    public static class CardSelect
    {
        public class Response : IBackgroundQueueResponse
        {
            public int GameId { get; set; }

            public IEnumerable<Card> Cards { get; set; }

            public bool QueueTask { get; set; }
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
                    .When(x => x.Cards != null)
                    .WithMessage("Select five unique cards.");
                base.RuleFor(x => x.Cards.Distinct().Count())
                    .Equal(5)
                    .When(x => (x.Cards?.Count() ?? 0) == 5)
                    .WithMessage("Cards should be unique.");
                base.RuleForEach(x => x.Cards)
                    .Must(x => AllCards.List.Any(y => y.Name == x))
                    .When(x => (x.Cards?.Distinct().Count() ?? 0) == 5)
                    .WithMessage("Invalid card.");
            }
        }

        public class RequestHandler : IRequestHandler<Request, Response>
        {
            private readonly TripleTriadDbContext context;

            private readonly IStepHandler<SelectCardsStep> selectCardHandler;

            public RequestHandler(TripleTriadDbContext context, IStepHandler<SelectCardsStep> selectCardHandler)
            {
                this.context = context;
                this.selectCardHandler = selectCardHandler;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var game = await this.context.Games.GetGameOrThrowAsync(request.GameId, cancellationToken);

                if (game.PlayerOneId != request.PlayerId
                    && game.PlayerTwoId != request.PlayerId)
                {
                    throw new GameInvalidPlayerException(request.GameId, request.PlayerId);
                }

                if (game.Status != GameStatus.ChooseCards)
                {
                    throw new GameHasInvalidStatusException(request.GameId, game.Status);
                }

                var gameData = JsonConvert.DeserializeObject<GameData>(game.Data);

                var isPlayerOne = game.PlayerOneId == request.PlayerId;

                try
                {
                    var displayName = isPlayerOne ? game.PlayerOne.DisplayName : game.PlayerTwo.DisplayName;
                    gameData = this.selectCardHandler.Run(gameData, isPlayerOne, displayName, request.Cards);
                }
                catch (GameDataException ex)
                {
                    throw new GameDataInvalidException(request.GameId, ex);
                }

                game.Data = gameData.ToJson();

                await this.context.SaveChangesAsync(cancellationToken);

                return new Response
                {
                    GameId = request.GameId,
                    Cards = isPlayerOne ? gameData.PlayerOneCards : gameData.PlayerTwoCards,
                    QueueTask = ((isPlayerOne ? gameData.PlayerTwoCards : gameData.PlayerOneCards)
                        ?.Count() ?? 0) == 5
                };
            }
        }

        public class GameStartBackgroundQueue : MediatorQueuePostProcessor<Request, Response, GameStart.Request, GameStart.Response>
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