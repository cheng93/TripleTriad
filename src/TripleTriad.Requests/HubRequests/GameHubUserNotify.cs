using Microsoft.AspNetCore.SignalR;
using TripleTriad.Data;
using TripleTriad.SignalR;

namespace TripleTriad.Requests.HubRequests
{
    public static class GameHubUserNotify
    {
        public class Request : GameHubNotify.Request
        {
            public string UserId { get; set; }
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
                    .User(request.UserId);
        }
    }
}