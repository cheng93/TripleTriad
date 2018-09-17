using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TripleTriad.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "players",
                columns: table => new
                {
                    player_id = table.Column<Guid>(nullable: false),
                    display_name = table.Column<string>(nullable: true),
                    account_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_players", x => x.player_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "players");
        }
    }
}
