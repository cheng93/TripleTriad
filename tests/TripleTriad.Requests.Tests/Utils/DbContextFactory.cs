using System;
using Microsoft.EntityFrameworkCore;
using TripleTriad.Data;

namespace TripleTriad.Requests.Tests.Utils
{
    public static class DbContextFactory
    {
        private static DbContextOptions<TripleTriadDbContext> CreateTripleTriadContextOptions()
            => new DbContextOptionsBuilder<TripleTriadDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

        public static TripleTriadDbContext CreateTripleTriadContext()
            => new TripleTriadDbContext(CreateTripleTriadContextOptions());
    }
}