using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;
using TripleTriad.Requests.Pipeline;
using TripleTriad.Data;
using TripleTriad.Logic.Entities;
using TripleTriad.Data.Entities;
using TripleTriad.Logic.Extensions;
using TripleTriad.Requests.Response;
using TripleTriad.BackgroundTasks.Queue;
using TripleTriad.Requests.HubRequests;
using TripleTriad.Requests.Messages;
using TripleTriad.SignalR;

namespace TripleTriad.Requests.GameRequests
{
    public static class GameCreate
    {
        public class Response : IGameResponse, IBackgroundQueueResponse, INotification
        {
            public int GameId { get; set; }

            public bool QueueTask => true;
        }

        public class Request : IRequest<Response>
        {
            public Guid PlayerId { get; set; }
        }

        public class Validator : ValidationPreProcessor<Request>
        {
            public Validator()
            {
                base.RuleFor(x => x.PlayerId).NotEmpty();
            }
        }

        public class RequestHandler : IRequestHandler<Request, Response>
        {
            private readonly TripleTriadDbContext context;

            public RequestHandler(TripleTriadDbContext context)
            {
                this.context = context;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var game = new Game
                {
                    HostId = request.PlayerId,
                    Data = new GameData().ToJson()
                };

                await this.context.AddAsync(game, cancellationToken);
                await this.context.SaveChangesAsync(cancellationToken);

                return new Response
                {
                    GameId = game.GameId
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

        public class LobbyNotifier : AsyncMediatorNotificationHandler<Response, HubGroupNotify.Request, Unit>
        {
            private readonly IMessageFactory<Messages.GameList.MessageData> messageFactory;

            public LobbyNotifier(
                IMediator mediator,
                IMessageFactory<Messages.GameList.MessageData> messageFactory)
                : base(mediator)
            {
                this.messageFactory = messageFactory;
            }

            protected async override Task<HubGroupNotify.Request> GetRequest(Response notification, CancellationToken cancellationToken)
                => new HubGroupNotify.Request
                {
                    Group = GameHub.Lobby,
                    Message = await this.messageFactory.Create(new Messages.GameList.MessageData())
                };
        }
    }
}