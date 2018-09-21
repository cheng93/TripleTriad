﻿using Microsoft.EntityFrameworkCore.Migrations;
using TripleTriad.Data.Enums;

namespace TripleTriad.Data.Migrations
{
    public partial class GameStatusEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:game_status", "waiting,in_progress,forfeit,player_one_win,player_two_win,tie");

            migrationBuilder.AddColumn<GameStatus>(
                name: "status",
                table: "games",
                nullable: false,
                defaultValue: GameStatus.Waiting);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "games");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:game_status", "waiting,in_progress,forfeit,player_one_win,player_two_win,tie");
        }
    }
}
