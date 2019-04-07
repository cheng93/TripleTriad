using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using Moq;
using TripleTriad.Common;
using TripleTriad.Requests.Jwt;
using TripleTriad.Requests.TokenRequests;
using Xunit;

namespace TripleTriad.Requests.Tests.TokenTests.TokenCreateTests
{
    public class RequestHandlerTests
    {
        private readonly TokenCreate.RequestHandler subject;

        private readonly Mock<IJwtWriter> jwtWriter = new Mock<IJwtWriter>();

        private readonly Mock<IJwtSigningKeyProvider> jwtSigningKeyProvider = new Mock<IJwtSigningKeyProvider>();

        private readonly SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("secret"));

        public RequestHandlerTests()
        {
            this.jwtSigningKeyProvider
                .Setup(x => x.GetKey(It.IsAny<CancellationToken>()))
                .ReturnsAsync(this.securityKey);

            this.subject = new TokenCreate.RequestHandler(
                this.jwtWriter.Object,
                this.jwtSigningKeyProvider.Object);
        }

        [Fact]
        public async Task Should_return_token()
        {
            var token = "token";
            this.jwtWriter
                .Setup(x => x.Write(It.IsAny<SecurityToken>()))
                .Returns(token);

            var actual = await this.subject.Handle(new TokenCreate.Request(), default);

            actual.Token.Should().Be(token);
        }

        [Fact]
        public async Task Should_write_valid_token()
        {
            var playerId = new Guid("58527a83-e920-44c2-8dac-9221f0c9cf09");
            this.jwtWriter
                .Setup(x => x.Write(It.IsAny<SecurityToken>()))
                .Verifiable();

            var request = new TokenCreate.Request
            {
                PlayerId = playerId
            };
            var actual = await this.subject.Handle(request, default);

            this.jwtWriter.Verify(
                x => x.Write(It.Is<JwtSecurityToken>(
                    t => t.Audiences.Contains(Constants.TripleTriad)
                        && t.Issuer == Constants.TripleTriad
                        && t.Claims.Any(
                            c => c.Type == Constants.Claims.PlayerId
                                && c.Value == playerId.ToString())
                        && t.SigningCredentials.Key == securityKey)));
        }
    }
}