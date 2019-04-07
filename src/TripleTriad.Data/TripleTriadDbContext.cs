using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Linq;
using TripleTriad.Data.Entities;
using TripleTriad.Data.Enums;
using TripleTriad.Data.Extensions;

namespace TripleTriad.Data
{
    public class TripleTriadDbContext : DbContext
    {
        static TripleTriadDbContext()
            => NpgsqlConnection.GlobalTypeMapper
                .MapEnum<GameStatus>(nameof(GameStatus).ToSnakeCase());

        public TripleTriadDbContext(DbContextOptions<TripleTriadDbContext> options)
            : base(options)
        {

        }

        public DbSet<Game> Games { get; set; }

        public DbSet<Player> Players { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("User ID=postgres;Host=localhost;Port=5432;Database=triple_triad");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.UseSnakeCaseNamingConvention();

            modelBuilder.ForNpgsqlHasEnum(
                nameof(GameStatus).ToSnakeCase(),
                Enum.GetValues(typeof(GameStatus))
                    .Cast<GameStatus>()
                    .Select(x => x.ToString().ToSnakeCase())
                    .ToArray());

            modelBuilder.Entity<Game>()
                .HasOne(g => g.Host)
                .WithMany()
                .HasForeignKey(g => g.HostId);

            modelBuilder.Entity<Game>()
                .HasOne(g => g.PlayerTwo)
                .WithMany()
                .HasForeignKey(g => g.PlayerTwoId);

            modelBuilder.Entity<Game>()
                .Property(g => g.Data)
                .HasColumnType("jsonb")
                .HasDefaultValue("{}")
                .IsRequired();
        }
    }
}
