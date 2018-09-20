using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using TripleTriad.Commands.Game;
using Xunit;

namespace TripleTriad.Commands.Tests.GameTests.GameCreateTests
{
    public class GameCreateValidatorTests
    {
        public static IEnumerable<object[]> BadRequests => new[]
        {
            new object[] { new GameCreate.Request() },
            new object[] { new GameCreate.Request() { PlayerId = Guid.Empty } }
        };

        [Theory]
        [MemberData(nameof(BadRequests))]
        public void Bad_request_should_throw(GameCreate.Request request)
        {
            var subject = new GameCreate.Validator();
            Func<Task> action = async () => await subject.Process(request, default);

            action.Should().Throw<ValidationException>();
        }

        public static IEnumerable<object[]> GoodRequests => new[]
        {
            new object[] { new GameCreate.Request() { PlayerId = Guid.NewGuid() } },
            new object[] { new GameCreate.Request() { PlayerId = new Guid("6faf20d7-eee5-482a-b8ff-73754f39cf19") } }
        };


        [Theory]
        [MemberData(nameof(GoodRequests))]
        public void Good_request_should_not_throw(GameCreate.Request request)
        {
            var subject = new GameCreate.Validator();
            Func<Task> action = async () => await subject.Process(request, default);

            action.Should().NotThrow<ValidationException>();
        }
    }
}