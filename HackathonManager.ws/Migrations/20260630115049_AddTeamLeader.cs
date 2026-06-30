using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HackathonManager.ws.Migrations
{
    /// <inheritdoc />
    public partial class AddTeamLeader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LeaderId",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_LeaderId",
                table: "Teams",
                column: "LeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_AspNetUsers_LeaderId",
                table: "Teams",
                column: "LeaderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_AspNetUsers_LeaderId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_LeaderId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "LeaderId",
                table: "Teams");
        }
    }
}
