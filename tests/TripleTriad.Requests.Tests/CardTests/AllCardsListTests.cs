using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using TripleTriad.Logic.Cards;
using TripleTriad.Requests.CardRequests;
using Xunit;

namespace TripleTriad.Requests.Tests.CardTests
{
    public class AllCardsListTests
    {
        [Fact]
        public async Task Should_return_all_cards()
        {
            var command = new AllCardsList.Request();
            var subject = new AllCardsList.RequestHandler();

            var response = await ((IRequestHandler<AllCardsList.Request, AllCardsList.Response>)subject).Handle(command, default);

            response.Cards.Should().BeEquivalentTo(AllCards.List);
        }
    }
}