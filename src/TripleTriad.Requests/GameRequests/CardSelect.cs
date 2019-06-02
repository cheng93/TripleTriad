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
using TripleTriad.Requests.Notifications;
using TripleTriad.Requests.Pipeline;
using TripleTriad.Requests.Response;

namespace TripleTriad.Requests.GameRequests
{
    public static class CardSelect
    {
        public class Response : ISendNotificationResponse, IGameResponse
        {
            public int GameId { get; set; }

            public IEnumerable<Card> Cards { get; set; }

            bool ISendNotificationResponse.QueueTask => this.queueTask;

            private bool queueTask;

            internal void SetQueueTask(bool queueTask) => this.queueTask = queueTask;
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

                if (game.HostId != request.PlayerId
                    && game.ChallengerId != request.PlayerId)
                {
                    throw new GameInvalidPlayerException(request.GameId, request.PlayerId);
                }

                if (game.Status != GameStatus.ChooseCards)
                {
                    throw new GameHasInvalidStatusException(request.GameId, game.Status);
                }

                var gameData = JsonConvert.DeserializeObject<GameData>(game.Data);

                var isHost = game.HostId == request.PlayerId;

                try
                {
                    var displayName = isHost ? game.Host.DisplayName : game.Challenger.DisplayName;
                    gameData = this.selectCardHandler.Run(gameData, isHost, displayName, request.Cards);
                }
                catch (GameDataException ex)
                {
                    throw new GameDataInvalidException(request.GameId, ex);
                }

                game.Data = gameData.ToJson();

                await this.context.SaveChangesAsync(cancellationToken);

                var response = new Response
                {
                    GameId = request.GameId,
                    Cards = isHost ? gameData.HostCards : gameData.ChallengerCards
                };

                response.SetQueueTask(
                    ((isHost ? gameData.ChallengerCards : gameData.HostCards)
                        ?.Count() ?? 0) == 5);

                return response;
            }
        }

        public class BackgroundEnqueuer : NotificationSenderPostProcessor<Request, Response>
        {
            public BackgroundEnqueuer(IBackgroundTaskQueue queue)
                : base(queue)
            {

            }

            protected override void SendNotifications(Request request, Response response)
            {
                this.Queue.QueueBackgroundTask(new GameStartNotification(response.GameId));
            }
        }
    }
}