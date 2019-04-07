﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TripleTriad.Data;
using TripleTriad.Data.Enums;

namespace TripleTriad.Data.Migrations
{
    [DbContext(typeof(TripleTriadDbContext))]
    [Migration("20190407162413_RenameColumns")]
    partial class RenameColumns
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:Enum:game_status", "waiting,choose_cards,in_progress,finished")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("TripleTriad.Data.Entities.Game", b =>
                {
                    b.Property<int>("GameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("game_id");

                    b.Property<Guid?>("ChallengerId")
                        .HasColumnName("challenger_id");

                    b.Property<string>("Data")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnName("data")
                        .HasColumnType("jsonb")
                        .HasDefaultValue("{}");

                    b.Property<Guid>("HostId")
                        .HasColumnName("host_id");

                    b.Property<GameStatus>("Status")
                        .HasColumnName("status");

                    b.HasKey("GameId")
                        .HasName("pk_games");

                    b.HasIndex("ChallengerId")
                        .HasName("ix_games_challenger_id");

                    b.HasIndex("HostId")
                        .HasName("ix_games_host_id");

                    b.ToTable("games");
                });

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

            modelBuilder.Entity("TripleTriad.Data.Entities.Game", b =>
                {
                    b.HasOne("TripleTriad.Data.Entities.Player", "Challenger")
                        .WithMany()
                        .HasForeignKey("ChallengerId")
                        .HasConstraintName("fk_games_players_challenger_id");

                    b.HasOne("TripleTriad.Data.Entities.Player", "Host")
                        .WithMany()
                        .HasForeignKey("HostId")
                        .HasConstraintName("fk_games_players_host_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
