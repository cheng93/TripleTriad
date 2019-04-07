using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using TripleTriad.Requests.TokenRequests;
using Xunit;

namespace TripleTriad.Requests.Tests.TokenTests.TokenCreateTests
{
    public class ValidatorTests
    {
        public static IEnumerable<object[]> BadRequests()
        {
            yield return new object[] { new TokenCreate.Request() };
            yield return new object[] { new TokenCreate.Request() { PlayerId = Guid.Empty } };
        }

        [Theory]
        [MemberData(nameof(BadRequests))]
        public void Bad_request_should_throw(TokenCreate.Request request)
        {
            var subject = new TokenCreate.Validator();
            Func<Task> action = async () => await subject.Process(request, default);

            action.Should().Throw<ValidationException>();
        }

        public static IEnumerable<object[]> GoodRequests()
        {
            yield return new object[] { new TokenCreate.Request() { PlayerId = new Guid("89c58344-e392-40e7-9973-19b35e090a32") } };
        }

        [Theory]
        [MemberData(nameof(GoodRequests))]
        public void Good_request_should_not_throw(TokenCreate.Request request)
        {
            var subject = new TokenCreate.Validator();
            Func<Task> action = async () => await subject.Process(request, default);

            action.Should().NotThrow<ValidationException>();
        }
    }
}