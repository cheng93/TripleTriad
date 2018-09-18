﻿using Microsoft.EntityFrameworkCore;
using System;
using TripleTriad.Data.Entities;
using TripleTriad.Data.Extensions;

namespace TripleTriad.Data
{
    public class TripleTriadDbContext : DbContext
    {
        public TripleTriadDbContext(DbContextOptions<TripleTriadDbContext> options)
            : base(options)
        {

        }

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
        }
    }
}
