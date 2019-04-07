using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TripleTriad.BackgroundTasks.Queue;
using TripleTriad.Data;
using TripleTriad.Data.Entities;
using TripleTriad.Data.Enums;
using TripleTriad.Logic.CoinToss;
using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Exceptions;
using TripleTriad.Logic.Extensions;
using TripleTriad.Logic.Steps;
using TripleTriad.Logic.Steps.Handlers;
using TripleTriad.Requests.Exceptions;
using TripleTriad.Requests.Extensions;
using TripleTriad.Requests.HubRequests;
using TripleTriad.Requests.Pipeline;
using TripleTriad.Requests.Response;
using TripleTriad.SignalR;

namespace TripleTriad.Requests.GameRequests
{
    public static class GameStart
    {
        public class Response : IGameResponse, IBackgroundQueueResponse, INotification
        {
            public int GameId { get; set; }

            public Guid StartPlayerId { get; set; }

            public bool QueueTask => true;
        }

        public class Request : IRequest<Response>
        {
            public int GameId { get; set; }
        }

        public class Validator : ValidationPreProcessor<Request>
        {
            public Validator()
            {
                base.RuleFor(x => x.GameId).NotEmpty();
            }
        }

        public class RequestHandler : IRequestHandler<Request, Response>
        {
            private readonly TripleTriadDbContext context;
            private readonly IStepHandler<CoinTossStep> coinTossHandler;
            private readonly IStepHandler<CreateBoardStep> createBoardHandler;

            public RequestHandler(
                TripleTriadDbContext context,
                IStepHandler<CoinTossStep> coinTossHandler,
                IStepHandler<CreateBoardStep> createBoardHandler)
            {
                this.context = context;
                this.coinTossHandler = coinTossHandler;
                this.createBoardHandler = createBoardHandler;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var game = await this.context.Games.GetGameOrThrowAsync(request.GameId, cancellationToken);

                if (game.Status != GameStatus.ChooseCards)
                {
                    throw new GameHasInvalidStatusException(request.GameId, game.Status);
                }

                var gameData = JsonConvert.DeserializeObject<GameData>(game.Data);
                try
                {

                    gameData = this.coinTossHandler.Run(gameData, game.Host.DisplayName, game.PlayerTwo.DisplayName);
                    gameData = this.createBoardHandler.Run(gameData);
                }
                catch (GameDataException ex)
                {
                    throw new GameDataInvalidException(request.GameId, ex);
                }

                game.Status = GameStatus.InProgress;
                game.Data = gameData.ToJson();

                await this.context.SaveChangesAsync(cancellationToken);

                return new Response
                {
                    GameId = request.GameId,
                    StartPlayerId = gameData.HostTurn.Value
                        ? game.HostId
                        : game.PlayerTwoId.Value
                };
            }
        }

        public class BackgroundEnqueuer : BackgroundQueuePostProcessor<Request, Response>
        {
            public BackgroundEnqueuer(IBackgroundTaskQueue queue)
                : base(queue)
            {

            }
        }

        public class GameHubGroupNotifyNotificationHandler : MediatorNotificationHandler<Response, HubRequests.GameHubGroupNotify.Request, Unit>
        {
            public GameHubGroupNotifyNotificationHandler(IMediator mediator)
                : base(mediator)
            {
            }

            protected override GameHubGroupNotify.Request GetRequest(Response notification)
                => new HubRequests.GameHubGroupNotify.Request
                {
                    GameId = notification.GameId
                };
        }
    }
}