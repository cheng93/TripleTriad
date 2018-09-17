﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TripleTriad.Data;

namespace TripleTriad.Data.Migrations
{
    [DbContext(typeof(TripleTriadDbContext))]
    partial class TripleTriadDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.2-rtm-30932")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("TripleTriad.Data.Entities.Player", b =>
                {
                    b.Property<Guid>("PlayerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("player_id");

                    b.Property<int?>("AccountId")
                        .HasColumnName("account_id");

                    b.Property<string>("DisplayName")
                        .HasColumnName("display_name");

                    b.HasKey("PlayerId")
                        .HasName("pk_players");

                    b.ToTable("players");
                });
#pragma warning restore 612, 618
        }
    }
}
