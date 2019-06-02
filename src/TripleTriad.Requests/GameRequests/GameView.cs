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
using TripleTriad.Requests.Notifications;
using TripleTriad.Requests.Pipeline;
using TripleTriad.Requests.Response;

namespace TripleTriad.Requests.GameRequests
{
    public static class GameView
    {
        public class Response : IGameResponse, ISendNotificationResponse
        {
            public int GameId { get; set; }

            public bool IsHost { get; set; }

            public bool IsChallenger { get; set; }

            public Guid PlayerId { get; set; }

            bool ISendNotificationResponse.QueueTask => true;
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

        public class BackgroundEnqueuer : NotificationSenderPostProcessor<Request, Response>
        {
            public BackgroundEnqueuer(IBackgroundTaskQueue queue)
                : base(queue)
            {

            }

            protected override void SendNotifications(Request request, Response response)
            {
                this.Queue.QueueBackgroundTask(new UserNotification(response.GameId, response.PlayerId));
            }
        }
    }
}