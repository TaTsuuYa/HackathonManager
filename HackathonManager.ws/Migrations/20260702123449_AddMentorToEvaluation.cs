using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HackathonManager.ws.Migrations
{
    /// <inheritdoc />
    public partial class AddMentorToEvaluation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MentorId",
                table: "Evaluations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Evaluations_MentorId",
                table: "Evaluations",
                column: "MentorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluations_AspNetUsers_MentorId",
                table: "Evaluations",
                column: "MentorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluations_AspNetUsers_MentorId",
                table: "Evaluations");

            migrationBuilder.DropIndex(
                name: "IX_Evaluations_MentorId",
                table: "Evaluations");

            migrationBuilder.DropColumn(
                name: "MentorId",
                table: "Evaluations");
        }
    }
}
