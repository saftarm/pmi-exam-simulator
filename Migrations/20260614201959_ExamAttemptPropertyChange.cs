using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestAPI.Migrations
{
    /// <inheritdoc />
    public partial class ExamAttemptPropertyChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExamTitle",
                table: "ExamAttempts");

            migrationBuilder.RenameColumn(
                name: "TotalQuesitons",
                table: "ExamAttempts",
                newName: "TotalQuestions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalQuestions",
                table: "ExamAttempts",
                newName: "TotalQuesitons");

            migrationBuilder.AddColumn<string>(
                name: "ExamTitle",
                table: "ExamAttempts",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
