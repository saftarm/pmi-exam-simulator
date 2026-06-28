using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestAPI.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceCorrectCountWithScorePoints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ScorePoints",
                table: "ExamAttempts",
                type: "numeric(8,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.Sql(
                """
                UPDATE "ExamAttempts"
                SET "ScorePoints" = "CorrectCount"::numeric
                WHERE "CorrectCount" IS NOT NULL;
                """);

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentageScore",
                table: "ExamAttempts",
                type: "numeric(5,2)",
                nullable: false,
                computedColumnSql: "CASE WHEN \"TotalQuestions\" = 0 THEN 0.0 ELSE (\"ScorePoints\"::numeric / \"TotalQuestions\"::numeric) * 100 END",
                stored: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,2)",
                oldComputedColumnSql: "CASE WHEN \"TotalQuestions\" = 0 THEN 0.0 ELSE (\"CorrectCount\"::numeric / \"TotalQuestions\"::numeric) * 100 END",
                oldStored: true);

            migrationBuilder.DropColumn(
                name: "CorrectCount",
                table: "ExamAttempts");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalCorrect",
                table: "DomainPerformances",
                type: "numeric(8,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScorePoints",
                table: "ExamAttempts");

            migrationBuilder.AddColumn<int>(
                name: "CorrectCount",
                table: "ExamAttempts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "TotalCorrect",
                table: "DomainPerformances",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(8,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentageScore",
                table: "ExamAttempts",
                type: "numeric(5,2)",
                nullable: false,
                computedColumnSql: "CASE WHEN \"TotalQuestions\" = 0 THEN 0.0 ELSE (\"CorrectCount\"::numeric / \"TotalQuestions\"::numeric) * 100 END",
                stored: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,2)",
                oldComputedColumnSql: "CASE WHEN \"TotalQuestions\" = 0 THEN 0.0 ELSE (\"ScorePoints\"::numeric / \"TotalQuestions\"::numeric) * 100 END",
                oldStored: true);
        }
    }
}
