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
using TripleTriad.Requests.Notifications;

namespace TripleTriad.Requests.GameRequests
{
    public static class GameCreate
    {
        public class Response : IGameResponse, ISendNotificationResponse, INotification
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

        public class BackgroundEnqueuer : NotificationSenderPostProcessor<Request, Response>
        {
            public BackgroundEnqueuer(IBackgroundTaskQueue queue)
                : base(queue)
            {

            }

            protected override void SendNotifications(Request request, Response response)
            {
                this.Queue.QueueBackgroundTask(new LobbyNotification());
            }
        }
    }
}