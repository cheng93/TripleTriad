using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;
using TripleTriad.BackgroundTasks.Queue;
using TripleTriad.Data;
using TripleTriad.Data.Enums;
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
    public static class GameStart
    {
        public class Response : IGameResponse, ISendNotificationResponse
        {
            public int GameId { get; set; }

            public Guid StartPlayerId { get; set; }

            bool ISendNotificationResponse.QueueTask => true;

            internal Guid HostId { get; set; }

            internal Guid ChallengerId { get; set; }
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

                    gameData = this.coinTossHandler.Run(gameData, game.Host.DisplayName, game.Challenger.DisplayName);
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
                        : game.ChallengerId.Value,
                    HostId = game.HostId,
                    ChallengerId = game.ChallengerId.Value,
                };
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
                this.Queue.QueueBackgroundTask(new RoomNotification(response.GameId));
                this.Queue.QueueBackgroundTask(new UserNotification(response.GameId, response.HostId));
                this.Queue.QueueBackgroundTask(new UserNotification(response.GameId, response.ChallengerId));
            }
        }
    }
}