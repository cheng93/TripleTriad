using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;
using TripleTriad.Requests.Pipeline;
using TripleTriad.Data;
using TripleTriad.Logic.Entities;

namespace TripleTriad.Requests.Game
{
    using Game = TripleTriad.Data.Entities.Game;

    public static class GameCreate
    {
        public class Response
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
                    Data = JsonConvert.SerializeObject(new GameData())
                };

                await this.context.AddAsync(game);
                await this.context.SaveChangesAsync();

                return new Response
                {
                    GameId = game.GameId
                };
            }
        }
    }
}