using System.Collections.Generic;
using MediatR;
using TripleTriad.Logic.Cards;
using TripleTriad.Logic.Entities;

namespace TripleTriad.Requests.CardRequests
{
    public static class AllCardsList
    {
        public class Response
        {
            public IEnumerable<Card> Cards { get; set; }
        }

        public class Request : IRequest<Response> { }

        public class RequestHandler : RequestHandler<Request, Response>
        {
            protected override Response Handle(Request request)
            {
                return new Response
                {
                    Cards = AllCards.List
                };
            }
        }
    }
}