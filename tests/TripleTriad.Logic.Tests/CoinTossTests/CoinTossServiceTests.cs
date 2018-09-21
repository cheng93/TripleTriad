using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using TripleTriad.Logic.CoinToss;
using Xunit;

namespace TripleTriad.Logic.Tests.CoinTossTests
{
    public class CoinTossServiceTests
    {
        public static IEnumerable<object[]> HeadInputs
            => Enumerable.Range(0, 50)
                .Select(x => new object[] { x });

        public static IEnumerable<object[]> TailInputs
            => Enumerable.Range(50, 50)
                .Select(x => new object[] { x });

        [Theory]
        [MemberData(nameof(HeadInputs))]
        public void Should_return_true(int input)
        {
            var random = new Mock<IRandomWrapper>();
            random
                .Setup(x => x.Next(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(input);
            var subject = new CoinTossService(random.Object);

            var actual = subject.IsHeads();
            actual.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TailInputs))]
        public void Should_return_false(int input)
        {
            var random = new Mock<IRandomWrapper>();
            random
                .Setup(x => x.Next(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(input);
            var subject = new CoinTossService(random.Object);

            var actual = subject.IsHeads();
            actual.Should().BeFalse();
        }
    }
}