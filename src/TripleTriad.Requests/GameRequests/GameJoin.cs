using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TripleTriad.BackgroundTasks.Queue;
using TripleTriad.Data;
using TripleTriad.Data.Entities;
using TripleTriad.Data.Enums;
using TripleTriad.Requests.Exceptions;
using TripleTriad.Requests.Extensions;
using TripleTriad.Requests.HubRequests;
using TripleTriad.Requests.Messages;
using TripleTriad.Requests.Notifications;
using TripleTriad.Requests.Pipeline;
using TripleTriad.Requests.Response;

namespace TripleTriad.Requests.GameRequests
{
    public static class GameJoin
    {
        public class Response : ISendNotificationResponse, IGameResponse
        {
            public int GameId { get; set; }

            public bool QueueTask => true;
            internal Guid HostId { get; set; }
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

                if (game.ChallengerId != null)
                {
                    throw new CannotJoinGameException(request.GameId);
                }

                if (game.HostId == request.PlayerId)
                {
                    throw new CannotPlayYourselfException(
                        request.GameId,
                        request.PlayerId);
                }

                game.ChallengerId = request.PlayerId;
                game.Status = GameStatus.ChooseCards;

                await this.context.SaveChangesAsync(cancellationToken);

                return new Response
                {
                    GameId = game.GameId,
                    HostId = game.HostId
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
                this.Queue.QueueBackgroundTask(new UserNotification(response.GameId, response.HostId));
                this.Queue.QueueBackgroundTask(new UserNotification(response.GameId, request.PlayerId));
                this.Queue.QueueBackgroundTask(new LobbyNotification());
            }
        }
    }
}