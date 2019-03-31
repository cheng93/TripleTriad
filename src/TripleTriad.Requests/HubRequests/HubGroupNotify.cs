using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using TripleTriad.SignalR;

namespace TripleTriad.Requests.HubRequests
{
    public static class HubGroupNotify
    {
        public class Request : HubNotify.Request
        {
            public string Group { get; set; }
        }

        public class Validator : HubNotify.Validator<Request>
        {
            public Validator()
            {
                base.RuleFor(x => x.Group).NotEmpty();
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
                    .Group(request.Group.ToString());
        }
    }
}