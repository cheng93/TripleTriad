using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TripleTriad.Data.Migrations
{
    public partial class GamePlayerTwoIdNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_games_players_player_two_id",
                table: "games");

            migrationBuilder.AlterColumn<Guid>(
                name: "player_two_id",
                table: "games",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "fk_games_players_player_two_id",
                table: "games",
                column: "player_two_id",
                principalTable: "players",
                principalColumn: "player_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_games_players_player_two_id",
                table: "games");

            migrationBuilder.AlterColumn<Guid>(
                name: "player_two_id",
                table: "games",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_games_players_player_two_id",
                table: "games",
                column: "player_two_id",
                principalTable: "players",
                principalColumn: "player_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
