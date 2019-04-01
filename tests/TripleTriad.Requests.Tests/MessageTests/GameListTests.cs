using System.Threading;
using System.Threading.Tasks;
using FluentAssertions.Json;
using MediatR;
using Moq;
using Newtonsoft.Json.Linq;
using TripleTriad.Requests.Messages;
using Xunit;

namespace TripleTriad.Requests.Tests.MessageTests
{
    public class GameListTests
    {
        [Fact]
        public async Task Should_return_game_list()
        {
            var mediator = new Mock<IMediator>();
            mediator
                .Setup(x => x.Send(
                    It.IsAny<GameRequests.GameList.Request>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GameRequests.GameList.Response
                {
                    GameIds = new[] { 1, 2, 3, 4 }
                });

            var subject = new GameList.MessageFactory(
                mediator.Object);

            var actual = await subject.Create(new GameList.MessageData());

            var expected = "{ \"gameIds\": [ 1, 2, 3, 4 ] }";

            JToken.Parse(actual).Should().BeEquivalentTo(JToken.Parse(expected));
        }
    }
}