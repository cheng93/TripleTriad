using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using TripleTriad.Requests.HubRequests;
using TripleTriad.SignalR;
using Xunit;

namespace TripleTriad.Requests.Tests.HubTests.HubGroupNotifyTests
{
    public class ValidatorTests
    {
        public static IEnumerable<object[]> BadRequests()
        {
            yield return new object[] { new HubGroupNotify.Request() };
            yield return new object[] { new HubGroupNotify.Request() { Group = "1" } };
            yield return new object[] { new HubGroupNotify.Request() { Group = GameHub.Lobby } };
            yield return new object[] { new HubGroupNotify.Request() { Message = "Hello World" } };
        }

        [Theory]
        [MemberData(nameof(BadRequests))]
        public void Bad_request_should_throw(HubGroupNotify.Request request)
        {
            var subject = new HubGroupNotify.Validator();
            Func<Task> action = async () => await subject.Process(request, default);

            action.Should().Throw<ValidationException>();
        }

        public static IEnumerable<object[]> GoodRequests => new[]
        {
            new object[]
            {
                new HubGroupNotify.Request()
                {
                    Group = "1",
                    Message = "Hello World"
                }
            },
            new object[]
            {
                new HubGroupNotify.Request()
                {
                    Group = GameHub.Lobby,
                    Message = "Hello World"
                }
            }
        };

        [Theory]
        [MemberData(nameof(GoodRequests))]
        public void Good_request_should_not_throw(HubGroupNotify.Request request)
        {
            var subject = new HubGroupNotify.Validator();
            Func<Task> action = async () => await subject.Process(request, default);

            action.Should().NotThrow<ValidationException>();
        }
    }
}