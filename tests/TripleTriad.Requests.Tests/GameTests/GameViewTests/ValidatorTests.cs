using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using TripleTriad.Requests.GameRequests;
using Xunit;

namespace TripleTriad.Requests.Tests.GameTests.GameViewTests
{
    public class ValidatorTests
    {
        public static IEnumerable<object[]> BadRequests => new[]
        {
            new object[] { new GameView.Request() },
            new object[] { new GameView.Request() { GameId = 1, PlayerId = Guid.Empty } },
            new object[] { new GameView.Request() { GameId = 0, PlayerId = Guid.NewGuid() } },
            new object[] { new GameView.Request() { GameId = 0, PlayerId = Guid.Empty } }
        };

        [Theory]
        [MemberData(nameof(BadRequests))]
        public void Bad_request_should_throw(GameView.Request request)
        {
            var subject = new GameView.Validator();
            Func<Task> action = async () => await subject.Process(request, default);

            action.Should().Throw<ValidationException>();
        }

        public static IEnumerable<object[]> GoodRequests => new[]
        {
            new object[] { new GameView.Request() { GameId = 1, PlayerId = Guid.NewGuid() } },
            new object[] { new GameView.Request() { GameId = 2, PlayerId = new Guid("6faf20d7-eee5-482a-b8ff-73754f39cf19") } }
        };


        [Theory]
        [MemberData(nameof(GoodRequests))]
        public void Good_request_should_not_throw(GameView.Request request)
        {
            var subject = new GameView.Validator();
            Func<Task> action = async () => await subject.Process(request, default);

            action.Should().NotThrow<ValidationException>();
        }
    }
}