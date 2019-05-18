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
using TripleTriad.Requests.Pipeline;
using TripleTriad.Requests.Response;

namespace TripleTriad.Requests.GameRequests
{
    public static class GameJoin
    {
        public class Response : IBackgroundQueueResponse, IGameResponse, INotification
        {
            public int GameId { get; set; }

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

        public class GroupNotifier : AsyncMediatorNotificationHandler<Response, HubGroupNotify.Request, Unit>
        {
            private readonly IMessageFactory<GameState.MessageData> messageFactory;

            public GroupNotifier(
                IMediator mediator,
                IMessageFactory<Messages.GameState.MessageData> messageFactory)
                : base(mediator)
            {
                this.messageFactory = messageFactory;
            }

            protected async override Task<HubGroupNotify.Request> GetRequest(Response notification, CancellationToken cancellationToken)
                => new HubGroupNotify.Request
                {
                    Group = notification.GameId.ToString(),
                    Message = await this.messageFactory.Create(
                        new Messages.GameState.MessageData
                        {
                            GameId = notification.GameId
                        })
                };
        }
    }
}