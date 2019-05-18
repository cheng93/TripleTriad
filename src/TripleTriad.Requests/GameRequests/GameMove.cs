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
using TripleTriad.Requests.HubRequests;
using TripleTriad.Requests.Messages;
using TripleTriad.Requests.Pipeline;
using TripleTriad.Requests.Response;
using TripleTriad.SignalR;

namespace TripleTriad.Requests.GameRequests
{
    public static class GameMove
    {
        public class Response : IGameResponse, IBackgroundQueueResponse, INotification
        {
            public int GameId { get; set; }

            public IEnumerable<Tile> Tiles { get; set; }

            public IEnumerable<Card> Cards { get; set; }

            public bool QueueTask => true;
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
                    Tiles = gameData.Tiles
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

        public class RoomNotifier : AsyncMediatorNotificationHandler<Response, HubGroupNotify.Request, Unit>
        {
            private readonly TripleTriadDbContext context;
            private readonly IMessageFactory<Messages.GameState.MessageData> messageFactory;
            private readonly IConnectionIdStore connectionIdStore;

            public RoomNotifier(
                TripleTriadDbContext context,
                IMediator mediator,
                IMessageFactory<Messages.GameState.MessageData> messageFactory,
                IConnectionIdStore connectionIdStore)
                : base(mediator)
            {
                this.context = context;
                this.messageFactory = messageFactory;
                this.connectionIdStore = connectionIdStore;
            }

            protected async override Task<HubGroupNotify.Request> GetRequest(Response notification, CancellationToken cancellationToken)
            {
                var game = await this.context.Games.GetGameOrThrowAsync(notification.GameId, cancellationToken);
                var connectionIdTasks = Task.WhenAll(
                    new[]
                    {
                        game.HostId.ToString(),
                        game.ChallengerId.ToString()
                    }
                    .Select(x => this.connectionIdStore.GetConnectionIds(x)));

                return new HubGroupNotify.Request
                {
                    Group = notification.GameId.ToString(),
                    Message = await this.messageFactory.Create(
                        new Messages.GameState.MessageData
                        {
                            GameId = notification.GameId
                        }),
                    Excluded = (await connectionIdTasks).SelectMany(x => x)
                };
            }
        }
    }
}