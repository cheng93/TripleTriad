using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace TripleTriad.SignalR.Tests
{
    public class ConnectionIdStoreTests
    {
        private const string UserId = "3735cf1b-887a-4928-9b0a-d753352aa843";

        [Fact]
        public async Task Should_add_connection()
        {
            var subject = new ConnectionIdStore();

            await subject.Add(new ConnectionIdStoreEntry(UserId, "1"));
            var actual = await subject.GetConnectionIds(UserId);

            actual.Should().BeEquivalentTo("1");
        }

        [Fact]
        public async Task Should_add_connection_when_user_id_exists()
        {
            var subject = new ConnectionIdStore();

            await subject.Add(new ConnectionIdStoreEntry(UserId, "1"));
            await subject.Add(new ConnectionIdStoreEntry(UserId, "2"));
            var actual = await subject.GetConnectionIds(UserId);

            actual.Should().BeEquivalentTo("1", "2");
        }

        [Fact]
        public async Task Should_return_empty_when_no_connections()
        {
            var subject = new ConnectionIdStore();

            var actual = await subject.GetConnectionIds(UserId);

            actual.Should().BeEmpty();
        }

        [Fact]
        public async Task Should_return_empty_when_no_connections_via_removing()
        {
            var subject = new ConnectionIdStore();

            await subject.Add(new ConnectionIdStoreEntry(UserId, "1"));
            await subject.Remove(new ConnectionIdStoreEntry(UserId, "1"));
            var actual = await subject.GetConnectionIds(UserId);

            actual.Should().BeEmpty();
        }

        [Fact]
        public async Task Should_return_connection_via_removing()
        {
            var subject = new ConnectionIdStore();

            await subject.Add(new ConnectionIdStoreEntry(UserId, "1"));
            await subject.Add(new ConnectionIdStoreEntry(UserId, "2"));
            await subject.Remove(new ConnectionIdStoreEntry(UserId, "1"));
            var actual = await subject.GetConnectionIds(UserId);

            actual.Should().BeEquivalentTo("2");
        }
    }
}
