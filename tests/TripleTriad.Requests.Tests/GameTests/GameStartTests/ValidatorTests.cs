using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using TripleTriad.Requests.GameRequests;
using Xunit;

namespace TripleTriad.Requests.Tests.GameTests.GameStartTests
{
    public class ValidatorTests
    {
        public static IEnumerable<object[]> BadRequests => new[]
        {
            new object[] { new GameStart.Request() },
            new object[] { new GameStart.Request() { GameId = 0 } }
        };

        [Theory]
        [MemberData(nameof(BadRequests))]
        public void Bad_request_should_throw(GameStart.Request request)
        {
            var subject = new GameStart.Validator();
            Func<Task> action = async () => await subject.Process(request, default);

            action.Should().Throw<ValidationException>();
        }

        public static IEnumerable<object[]> GoodRequests => new[]
        {
            new object[] { new GameStart.Request() { GameId = 1 } },
            new object[] { new GameStart.Request() { GameId = 2 } }
        };


        [Theory]
        [MemberData(nameof(GoodRequests))]
        public void Good_request_should_not_throw(GameStart.Request request)
        {
            var subject = new GameStart.Validator();
            Func<Task> action = async () => await subject.Process(request, default);

            action.Should().NotThrow<ValidationException>();
        }
    }
}