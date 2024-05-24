using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedTournament : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TournamentId1",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Games_TournamentId1",
                table: "Games",
                column: "TournamentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Tournaments_TournamentId1",
                table: "Games",
                column: "TournamentId1",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Tournaments_TournamentId1",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_TournamentId1",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "TournamentId1",
                table: "Games");
        }
    }
}
