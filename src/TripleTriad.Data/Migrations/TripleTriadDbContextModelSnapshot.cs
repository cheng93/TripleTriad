﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TripleTriad.Data;
using TripleTriad.Data.Enums;

namespace TripleTriad.Data.Migrations
{
    [DbContext(typeof(TripleTriadDbContext))]
    partial class TripleTriadDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:Enum:game_status", "waiting,choose_cards,in_progress,finished")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("TripleTriad.Data.Entities.Game", b =>
                {
                    b.Property<int>("GameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("game_id");

                    b.Property<string>("Data")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnName("data")
                        .HasColumnType("jsonb")
                        .HasDefaultValue("{}");

                    b.Property<Guid>("PlayerOneId")
                        .HasColumnName("player_one_id");

                    b.Property<Guid?>("PlayerTwoId")
                        .HasColumnName("player_two_id");

                    b.Property<GameStatus>("Status")
                        .HasColumnName("status");

                    b.HasKey("GameId")
                        .HasName("pk_games");

                    b.HasIndex("PlayerOneId")
                        .HasName("ix_games_player_one_id");

                    b.HasIndex("PlayerTwoId")
                        .HasName("ix_games_player_two_id");

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
                    b.HasOne("TripleTriad.Data.Entities.Player", "PlayerOne")
                        .WithMany()
                        .HasForeignKey("PlayerOneId")
                        .HasConstraintName("fk_games_players_player_one_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TripleTriad.Data.Entities.Player", "PlayerTwo")
                        .WithMany()
                        .HasForeignKey("PlayerTwoId")
                        .HasConstraintName("fk_games_players_player_two_id");
                });
#pragma warning restore 612, 618
        }
    }
}
