using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using TripleTriad.Requests.GameRequests;
using Xunit;

namespace TripleTriad.Requests.Tests.GameTests.GameJoinTests
{
    public class ValidatorTests
    {
        public static IEnumerable<object[]> BadRequests => new[]
        {
            new object[] { new GameJoin.Request() },
            new object[] { new GameJoin.Request() { PlayerId = Guid.Empty } },
            new object[] { new GameJoin.Request() { GameId = 0 } },
            new object[] { new GameJoin.Request() { GameId = 0, PlayerId = Guid.Empty } }
        };

        [Theory]
        [MemberData(nameof(BadRequests))]
        public void Bad_request_should_throw(GameJoin.Request request)
        {
            var subject = new GameJoin.Validator();
            Func<Task> action = async () => await subject.Process(request, default);

            action.Should().Throw<ValidationException>();
        }

        public static IEnumerable<object[]> GoodRequests => new[]
        {
            new object[] { new GameJoin.Request() { GameId = 1, PlayerId = Guid.NewGuid() } },
            new object[] { new GameJoin.Request() { GameId = 2, PlayerId = new Guid("6faf20d7-eee5-482a-b8ff-73754f39cf19") } }
        };


        [Theory]
        [MemberData(nameof(GoodRequests))]
        public void Good_request_should_not_throw(GameJoin.Request request)
        {
            var subject = new GameJoin.Validator();
            Func<Task> action = async () => await subject.Process(request, default);

            action.Should().NotThrow<ValidationException>();
        }
    }
}