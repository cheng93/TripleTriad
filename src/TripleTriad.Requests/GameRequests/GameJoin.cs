using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TripleTriad.Data;
using TripleTriad.Data.Entities;
using TripleTriad.Requests.GameRequests.Exceptions;
using TripleTriad.Requests.Pipeline;

namespace TripleTriad.Requests.GameRequests
{
    public static class GameJoin
    {
        public class Response
        {
            public int GameId { get; set; }
        }

        public class Request : IRequest<Response>
        {
            public int GameId { get; set; }

            public Guid PlayerId { get; set; }
        }

        public class Validator : ValidationPreProcessor<Request>
        {
            public Validator()
            {
                base.RuleFor(x => x.GameId).NotEmpty();

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
                Game game;
                var gameExists = await this.context.Games.AnyAsync(
                    x => x.GameId == request.GameId,
                    cancellationToken);
                if (!gameExists)
                {
                    throw new GameNotFoundException(request.GameId);
                }
                try
                {
                    game = await this.context.Games.SingleAsync(
                    x => x.GameId == request.GameId
                        && x.PlayerTwoId == null,
                    cancellationToken);
                }
                catch (Exception)
                {
                    throw new CannotJoinGameException(request.GameId);
                }

                game.PlayerTwoId = request.PlayerId;

                await this.context.SaveChangesAsync(cancellationToken);

                return new Response
                {
                    GameId = game.GameId
                };
            }
        }
    }
}