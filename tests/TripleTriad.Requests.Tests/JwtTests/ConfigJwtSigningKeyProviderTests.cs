using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using TripleTriad.Requests.Jwt;
using Xunit;

namespace TripleTriad.Requests.Tests.JwtTests
{
    public class ConfigJwtSigningKeyProviderTests
    {
        [Fact]
        public async Task Should_return_symmetric_key()
        {
            var secret = "secret";

            var configuration = new Mock<IConfiguration>();

            configuration
                .Setup(x => x[It.IsAny<string>()])
                .Returns(secret);

            var subject = new ConfigJwtSigningKeyProvider(configuration.Object);

            var actual = await subject.GetKey(default);

            actual.Key.Should().BeEquivalentTo(Encoding.ASCII.GetBytes(secret));
        }
    }
}