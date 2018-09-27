using Microsoft.AspNetCore.SignalR;
using TripleTriad.Data;
using TripleTriad.SignalR;

namespace TripleTriad.Requests.GameRequests
{
    public static class GameNotifyGroup
    {
        public class Request : GameNotify.Request
        {
        }

        public class RequestHandler : GameNotify.RequestHandler<Request>
        {
            private readonly IHubContext<GameHub, IGameClient> hubContext;

            public RequestHandler(
                TripleTriadDbContext dbContext,
                IHubContext<GameHub, IGameClient> hubContext)
                : base(dbContext)
            {
                this.hubContext = hubContext;
            }

            protected override IGameClient GetGameClient(Request request)
                => this.hubContext
                    .Clients
                    .Group(request.GameId.ToString());

        }
    }
}