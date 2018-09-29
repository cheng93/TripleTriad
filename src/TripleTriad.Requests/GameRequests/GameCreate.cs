using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;
using TripleTriad.Requests.Pipeline;
using TripleTriad.Data;
using TripleTriad.Logic.Entities;
using TripleTriad.Data.Entities;
using TripleTriad.Logic.Extensions;
using TripleTriad.Requests.Response;

namespace TripleTriad.Requests.GameRequests
{
    public static class GameCreate
    {
        public class Response : IGameResponse
        {
            public int GameId { get; set; }
        }

        public class Request : IRequest<Response>
        {
            public Guid PlayerId { get; set; }
        }

        public class Validator : ValidationPreProcessor<Request>
        {
            public Validator()
            {
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
                var game = new Game
                {
                    PlayerOneId = request.PlayerId,
                    Data = new GameData().ToJson()
                };

                await this.context.AddAsync(game, cancellationToken);
                await this.context.SaveChangesAsync(cancellationToken);

                return new Response
                {
                    GameId = game.GameId
                };
            }
        }
    }
}