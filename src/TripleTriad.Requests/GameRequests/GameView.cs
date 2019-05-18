using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using TripleTriad.BackgroundTasks.Queue;
using TripleTriad.Data;
using TripleTriad.Requests.Extensions;
using TripleTriad.Requests.HubRequests;
using TripleTriad.Requests.Messages;
using TripleTriad.Requests.Pipeline;
using TripleTriad.Requests.Response;

namespace TripleTriad.Requests.GameRequests
{
    public static class GameView
    {
        public class Response : IGameResponse, IBackgroundQueueResponse, INotification
        {
            public int GameId { get; set; }

            public bool IsHost { get; set; }

            public bool IsChallenger { get; set; }

            public Guid PlayerId { get; set; }

            public bool QueueTask => true;
        }

        public class Request : IRequest<Response>
        {
            public int GameId { get; set; }

            public Guid PlayerId { get; set; }
        }

        public class Validator : ValidationPreProcessor<Request>
        {
            public Validator()
            {
                base.RuleFor(x => x.GameId).NotEmpty();
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
                var game = await this.context.Games.GetGameOrThrowAsync(request.GameId, cancellationToken);

                return new Response
                {
                    GameId = request.GameId,
                    IsHost = game.HostId == request.PlayerId,
                    IsChallenger = game.ChallengerId == request.PlayerId,
                    PlayerId = request.PlayerId
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

        public class UserNotifier : AsyncMediatorNotificationHandler<Response, HubUserNotify.Request, Unit>
        {
            private readonly IMessageFactory<Messages.GameState.MessageData> messageFactory;
            public UserNotifier(
                IMediator mediator,
                IMessageFactory<Messages.GameState.MessageData> messageFactory)
                : base(mediator)
            {
                this.messageFactory = messageFactory;
            }

            protected async override Task<HubUserNotify.Request> GetRequest(Response notification, CancellationToken cancellationToken)
                => new HubUserNotify.Request
                {
                    UserId = notification.PlayerId,
                    Message = await this.messageFactory.Create(
                        new Messages.GameState.MessageData
                        {
                            GameId = notification.GameId,
                            PlayerId = notification.PlayerId
                        })
                };
        }
    }
}