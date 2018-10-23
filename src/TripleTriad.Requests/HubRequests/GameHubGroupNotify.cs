using Microsoft.AspNetCore.SignalR;
using TripleTriad.Data;
using TripleTriad.SignalR;

namespace TripleTriad.Requests.HubRequests
{
    public static class GameHubGroupNotify
    {
        public class Request : GameHubNotify.Request
        {
        }

        public class RequestHandler : GameHubNotify.RequestHandler<Request>
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