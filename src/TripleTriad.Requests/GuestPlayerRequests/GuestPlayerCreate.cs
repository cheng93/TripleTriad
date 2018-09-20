using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TripleTriad.Data;
using TripleTriad.Data.Entities;

namespace TripleTriad.Requests.GuestPlayerRequests
{
    public static class GuestPlayerCreate
    {
        public class Response
        {
            public Guid PlayerId { get; set; }
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
                var playersCount = await this.context.Players.CountAsync();
                var player = new Player()
                {
                    DisplayName = $"Guest{playersCount + 1}"
                };
                await this.context.AddAsync(player);
                await this.context.SaveChangesAsync();

                return new Response
                {
                    PlayerId = player.PlayerId
                };
            }
        }
    }
}