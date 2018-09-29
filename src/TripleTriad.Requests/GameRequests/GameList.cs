using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TripleTriad.Data;
using TripleTriad.Data.Enums;

namespace TripleTriad.Requests.GameRequests
{
    public static class GameList
    {
        public class Response
        {
            public IEnumerable<int> GameIds { get; set; }
        }

        public class Request : IRequest<Response> { }

        public class RequestHandler : IRequestHandler<Request, Response>
        {
            private readonly TripleTriadDbContext context;

            public RequestHandler(TripleTriadDbContext context)
            {
                this.context = context;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var games = await this.context
                    .Games
                    .Where(x => x.Status == GameStatus.Waiting)
                    .Select(x => x.GameId)
                    .ToListAsync(cancellationToken);

                return new Response
                {
                    GameIds = games
                };
            }
        }
    }
}