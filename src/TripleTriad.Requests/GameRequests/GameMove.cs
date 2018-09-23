using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using MediatR;
using TripleTriad.Logic.Cards;
using TripleTriad.Logic.Entities;
using TripleTriad.Requests.Pipeline;

namespace TripleTriad.Requests.GameRequests
{
    public static class GameMove
    {
        public class Response
        {
            public int GameId { get; set; }

            public IEnumerable<Tile> Tiles { get; set; }

            public IEnumerable<Card> Cards { get; set; }
        }

        public class Request : IRequest<Response>
        {
            public int GameId { get; set; }

            public Guid PlayerId { get; set; }

            public string Card { get; set; }

            public int TileId { get; set; }
        }

        public class Validator : ValidationPreProcessor<Request>
        {
            public Validator()
            {
                base.RuleFor(x => x.GameId).NotEmpty();
                base.RuleFor(x => x.PlayerId).NotEmpty();
                base.RuleFor(x => x.Card)
                    .Must(x => AllCards.List.Any(y => y.Name == x))
                    .WithMessage("Select a valid card.");
                base.RuleFor(x => x.TileId).InclusiveBetween(0, 8);
            }
        }
    }
}