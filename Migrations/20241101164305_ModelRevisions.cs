using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniGameV4.Migrations
{
    /// <inheritdoc />
    public partial class ModelRevisions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "GameRecord",
                newName: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GameRecord_UserId",
                table: "GameRecord",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameRecord_User_UserId",
                table: "GameRecord",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameRecord_User_UserId",
                table: "GameRecord");

            migrationBuilder.DropIndex(
                name: "IX_GameRecord_UserId",
                table: "GameRecord");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "GameRecord",
                newName: "UserID");
        }
    }
}
