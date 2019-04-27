using System;
using FluentValidation;
using Microsoft.AspNetCore.SignalR;
using TripleTriad.SignalR;

namespace TripleTriad.Requests.HubRequests
{
    public static class HubUserNotify
    {
        public class Request : HubNotify.Request
        {
            public Guid UserId { get; set; }
        }

        public class Validator : HubNotify.Validator<Request>
        {
            public Validator()
            {
                base.RuleFor(x => x.UserId).NotEmpty();
            }
        }

        public class RequestHandler : HubNotify.RequestHandler<Request>
        {
            private readonly IHubContext<GameHub, IGameClient> hubContext;

            public RequestHandler(IHubContext<GameHub, IGameClient> hubContext)
            {
                this.hubContext = hubContext;
            }

            protected override IGameClient GetGameClient(Request request)
                => this.hubContext
                    .Clients
                    .User(request.UserId.ToString());
        }
    }
}