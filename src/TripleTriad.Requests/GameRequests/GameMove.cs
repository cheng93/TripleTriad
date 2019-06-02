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
    public static class GameMove
    {
        public class Response : IGameResponse, ISendNotificationResponse
        {
            public int GameId { get; set; }

            public IEnumerable<Tile> Tiles { get; set; }

            public IEnumerable<Card> Cards { get; set; }

            bool ISendNotificationResponse.QueueTask => true;

            internal Guid HostId { get; set; }

            internal Guid ChallengerId { get; set; }
        }

        public class Request : IRequest<Response>
        {
            public int GameId { get; set; }

            public Guid PlayerId { get; set; }

            public string Card { get; set; }

            public int TileId { get; set; }
        }

        public class Validator : ValidationPreProcessor<Request>
        {
            public Validator()
            {
                base.RuleFor(x => x.GameId).NotEmpty();
                base.RuleFor(x => x.PlayerId).NotEmpty();
                base.RuleFor(x => x.Card)
                    .Must(x => AllCards.List.Any(y => y.Name == x))
                    .WithMessage("Select a valid card.");
                base.RuleFor(x => x.TileId).InclusiveBetween(0, 8);
            }
        }

        public class RequestHandler : IRequestHandler<Request, Response>
        {
            private readonly TripleTriadDbContext context;
            private readonly IStepHandler<PlayCardStep> playCardHandler;

            public RequestHandler(
                TripleTriadDbContext context,
                IStepHandler<PlayCardStep> playCardHandler)
            {
                this.context = context;
                this.playCardHandler = playCardHandler;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var game = await this.context.Games.GetGameOrThrowAsync(request.GameId, cancellationToken);

                if (game.HostId != request.PlayerId
                    && game.ChallengerId != request.PlayerId)
                {
                    throw new GameInvalidPlayerException(request.GameId, request.PlayerId);
                }

                if (game.Status != GameStatus.InProgress)
                {
                    throw new GameHasInvalidStatusException(request.GameId, game.Status);
                }

                var gameData = JsonConvert.DeserializeObject<GameData>(game.Data);
                var isHost = request.PlayerId == game.HostId;
                try
                {
                    gameData = this.playCardHandler.Run(
                        gameData,
                        request.Card,
                        request.TileId,
                        isHost);
                }
                catch (GameDataException ex)
                {
                    throw new GameDataInvalidException(request.GameId, ex);
                }

                game.Data = gameData.ToJson();
                if (gameData.Result != null)
                {
                    game.Status = GameStatus.Finished;
                }

                await this.context.SaveChangesAsync(cancellationToken);

                return new Response
                {
                    GameId = request.GameId,
                    Cards = isHost ? gameData.HostCards : gameData.ChallengerCards,
                    Tiles = gameData.Tiles,
                    HostId = game.HostId,
                    ChallengerId = game.ChallengerId.Value
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