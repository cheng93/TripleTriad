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
using TripleTriad.Requests.Pipeline;
using TripleTriad.Requests.Response;

namespace TripleTriad.Requests.GameRequests
{
    public static class GameMove
    {
        public class Response : IGameResponse, IBackgroundQueueResponse
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

                if (game.PlayerOneId != request.PlayerId
                    && game.PlayerTwoId != request.PlayerId)
                {
                    throw new GameInvalidPlayerException(request.GameId, request.PlayerId);
                }

                if (game.Status != GameStatus.InProgress)
                {
                    throw new GameHasInvalidStatusException(request.GameId, game.Status);
                }

                var gameData = JsonConvert.DeserializeObject<GameData>(game.Data);
                var isPlayerOne = request.PlayerId == game.PlayerOneId;
                try
                {
                    gameData = this.playCardHandler.Run(
                        gameData,
                        request.Card,
                        request.TileId,
                        isPlayerOne);
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
                    Cards = isPlayerOne ? gameData.PlayerOneCards : gameData.PlayerTwoCards,
                    Tiles = gameData.Tiles
                };
            }
        }

        public class GameHubGroupNotifyPostProcessor : MediatorQueuePostProcessor<Request, Response, GameHubGroupNotify.Request, Unit>
        {
            public GameHubGroupNotifyPostProcessor(IBackgroundTaskQueue queue, IMediator mediator)
                : base(queue, mediator)
            {
            }

            protected override GameHubGroupNotify.Request CreateQueueRequest(Request request, Response response)
                => new GameHubGroupNotify.Request
                {
                    GameId = response.GameId
                };
        }
    }
}