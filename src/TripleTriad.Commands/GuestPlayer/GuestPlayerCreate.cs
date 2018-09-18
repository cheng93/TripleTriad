using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TripleTriad.Data;
using TripleTriad.Data.Entities;

namespace TripleTriad.Commands.GuestPlayer
{
    public static class GuestPlayerCreate
    {
        public class Response
        {
            public Guid PlayerId { get; set; }
        }

        public class Command : IRequest<Response> { }

        public class CommandHandler : IRequestHandler<Command, Response>
        {
            private readonly TripleTriadDbContext context;

            public CommandHandler(TripleTriadDbContext context)
            {
                this.context = context;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var playersCount = await this.context.Players.CountAsync();
                var player = new Player()
                {
                    DisplayName = $"Guest{playersCount + 1}"
                };
                await context.AddAsync(player);
                await context.SaveChangesAsync();

                return new Response
                {
                    PlayerId = player.PlayerId
                };
            }
        }
    }
}