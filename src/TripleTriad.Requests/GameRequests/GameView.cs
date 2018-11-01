using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using TripleTriad.Data;
using TripleTriad.Requests.Extensions;
using TripleTriad.Requests.Pipeline;

namespace TripleTriad.Requests.GameRequests
{
    public static class GameView
    {
        public class Response
        {
            public bool IsPlayerOne { get; set; }

            public bool IsPlayerTwo { get; set; }
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
                var game = await this.context.Games.GetGameOrThrowAsync(request.GameId, cancellationToken);

                return new Response
                {
                    IsPlayerOne = game.PlayerOneId == request.PlayerId,
                    IsPlayerTwo = game.PlayerTwoId == request.PlayerId
                };
            }
        }
    }
}