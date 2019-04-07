using Microsoft.EntityFrameworkCore.Migrations;

namespace TripleTriad.Data.Migrations
{
    public partial class RenameColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_games_players_player_one_id",
                table: "games");

            migrationBuilder.DropForeignKey(
                name: "fk_games_players_player_two_id",
                table: "games");

            migrationBuilder.RenameColumn(
                name: "player_two_id",
                table: "games",
                newName: "challenger_id");

            migrationBuilder.RenameColumn(
                name: "player_one_id",
                table: "games",
                newName: "host_id");

            migrationBuilder.RenameIndex(
                name: "ix_games_player_two_id",
                table: "games",
                newName: "ix_games_challenger_id");

            migrationBuilder.RenameIndex(
                name: "ix_games_player_one_id",
                table: "games",
                newName: "ix_games_host_id");

            migrationBuilder.AddForeignKey(
                name: "fk_games_players_challenger_id",
                table: "games",
                column: "challenger_id",
                principalTable: "players",
                principalColumn: "player_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_games_players_host_id",
                table: "games",
                column: "host_id",
                principalTable: "players",
                principalColumn: "player_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_games_players_challenger_id",
                table: "games");

            migrationBuilder.DropForeignKey(
                name: "fk_games_players_host_id",
                table: "games");

            migrationBuilder.RenameColumn(
                name: "host_id",
                table: "games",
                newName: "player_one_id");

            migrationBuilder.RenameColumn(
                name: "challenger_id",
                table: "games",
                newName: "player_two_id");

            migrationBuilder.RenameIndex(
                name: "ix_games_host_id",
                table: "games",
                newName: "ix_games_player_one_id");

            migrationBuilder.RenameIndex(
                name: "ix_games_challenger_id",
                table: "games",
                newName: "ix_games_player_two_id");

            migrationBuilder.AddForeignKey(
                name: "fk_games_players_player_one_id",
                table: "games",
                column: "player_one_id",
                principalTable: "players",
                principalColumn: "player_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_games_players_player_two_id",
                table: "games",
                column: "player_two_id",
                principalTable: "players",
                principalColumn: "player_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
