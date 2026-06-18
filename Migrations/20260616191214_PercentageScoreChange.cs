using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestAPI.Migrations
{
    /// <inheritdoc />
    public partial class PercentageScoreChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "ExamAttempts");

            migrationBuilder.AddColumn<decimal>(
                name: "PercentageScore",
                table: "ExamAttempts",
                type: "numeric(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PercentageScore",
                table: "ExamAttempts");

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "ExamAttempts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
