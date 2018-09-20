using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TripleTriad.Commands.GuestPlayer;
using TripleTriad.Data;
using TripleTriad.Data.Entities;
using Xunit;

namespace TripleTriad.Commands.Tests.GuestPlayerTests
{
    public class GuestPlayerCreateTests
    {
        private DbContextOptions<TripleTriadDbContext> contextOptions
            => new DbContextOptionsBuilder<TripleTriadDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

        [Fact]
        public async Task Should_create_player()
        {
            var context = new TripleTriadDbContext(this.contextOptions);
            var command = new GuestPlayerCreate.Request();
            var subject = new GuestPlayerCreate.RequestHandler(context);

            var response = await subject.Handle(command, default(CancellationToken));

            Func<Task<Player>> act = async () => await context.Players.SingleAsync(x => x.PlayerId == response.PlayerId);

            act.Should().NotThrow();
        }

        [Fact]
        public async Task Created_player_should_have_guest_name()
        {
            var context = new TripleTriadDbContext(this.contextOptions);
            var command = new GuestPlayerCreate.Request();
            var subject = new GuestPlayerCreate.RequestHandler(context);

            var response = await subject.Handle(command, default(CancellationToken));

            var player = await context.Players.SingleAsync(x => x.PlayerId == response.PlayerId);

            player.DisplayName.Should().StartWith("Guest");
        }

        [Fact]
        public async Task Created_player_should_have_player_count_suffix()
        {
            var context = new TripleTriadDbContext(this.contextOptions);
            var command = new GuestPlayerCreate.Request();
            var subject = new GuestPlayerCreate.RequestHandler(context);

            await context.Players.AddRangeAsync(new[] {
                new Player(),
                new Player()
            });
            await context.SaveChangesAsync();

            var response = await subject.Handle(command, default(CancellationToken));

            var player = await context.Players.SingleAsync(x => x.PlayerId == response.PlayerId);

            player.DisplayName.Should().Be("Guest3");
        }
    }
}