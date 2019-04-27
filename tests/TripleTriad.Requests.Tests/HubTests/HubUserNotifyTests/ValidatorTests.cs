using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using TripleTriad.Requests.HubRequests;
using Xunit;

namespace TripleTriad.Requests.Tests.HubTests.HubUserNotifyTests
{
    public class ValidatorTests
    {
        public static IEnumerable<object[]> BadRequests()
        {
            yield return new object[] { new HubUserNotify.Request() };
            yield return new object[] { new HubUserNotify.Request() { UserId = Guid.NewGuid() } };
            yield return new object[] { new HubUserNotify.Request() { UserId = Guid.Empty } };
            yield return new object[] { new HubUserNotify.Request() { Message = "Hello World" } };
            yield return new object[] { new HubUserNotify.Request() { UserId = Guid.Empty, Message = "Hello World" } };
        }

        [Theory]
        [MemberData(nameof(BadRequests))]
        public void Bad_request_should_throw(HubUserNotify.Request request)
        {
            var subject = new HubUserNotify.Validator();
            Func<Task> action = async () => await subject.Process(request, default);

            action.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Good_request_should_not_throw()
        {
            var request = new HubUserNotify.Request()
            {
                UserId = Guid.NewGuid(),
                Message = "Hello World"
            };
            var subject = new HubUserNotify.Validator();
            Func<Task> action = async () => await subject.Process(request, default);

            action.Should().NotThrow<ValidationException>();
        }
    }
}