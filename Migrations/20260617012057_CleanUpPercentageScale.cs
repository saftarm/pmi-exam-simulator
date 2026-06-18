using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestAPI.Migrations
{
    /// <inheritdoc />
    public partial class CleanUpPercentageScale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PercentageScore",
                table: "ExamAttempts",
                type: "numeric(5,2)",
                nullable: false,
                computedColumnSql: "CASE WHEN \"TotalQuestions\" = 0 THEN 0.0 ELSE (\"CorrectCount\"::numeric / \"TotalQuestions\"::numeric) * 100 END",
                stored: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldComputedColumnSql: "CASE WHEN \"TotalQuestions\" = 0 THEN 0.0 ELSE (\"CorrectCount\"::numeric / \"TotalQuestions\"::numeric) * 100 END",
                oldStored: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PercentageScore",
                table: "ExamAttempts",
                type: "numeric",
                nullable: false,
                computedColumnSql: "CASE WHEN \"TotalQuestions\" = 0 THEN 0.0 ELSE (\"CorrectCount\"::numeric / \"TotalQuestions\"::numeric) * 100 END",
                stored: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,2)",
                oldComputedColumnSql: "CASE WHEN \"TotalQuestions\" = 0 THEN 0.0 ELSE (\"CorrectCount\"::numeric / \"TotalQuestions\"::numeric) * 100 END",
                oldStored: true);
        }
    }
}
