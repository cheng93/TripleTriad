using System;
using FluentValidation;
using MediatR;
using TripleTriad.Commands.Pipeline;

namespace TripleTriad.Commands.Game
{
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
    }
}