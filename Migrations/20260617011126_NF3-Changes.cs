using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestAPI.Migrations
{
    /// <inheritdoc />
    public partial class NF3Changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerOptions_Exams_ExamId",
                table: "AnswerOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_DomainPerformances_Domains_DomainId",
                table: "DomainPerformances");

            migrationBuilder.DropForeignKey(
                name: "FK_DomainPerformances_Exams_ExamId",
                table: "DomainPerformances");

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentageScore",
                table: "ExamAttempts",
                type: "numeric",
                nullable: false,
                computedColumnSql: "CASE WHEN \"TotalQuestions\" = 0 THEN 0.0 ELSE (\"CorrectCount\"::numeric / \"TotalQuestions\"::numeric) * 100 END",
                stored: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,2)",
                oldPrecision: 5,
                oldScale: 2);

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerOptions_Questions_QuestionId",
                table: "AnswerOptions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DomainPerformances_Domains_DomainId",
                table: "DomainPerformances",
                column: "DomainId",
                principalTable: "Domains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DomainPerformances_Exams_ExamId",
                table: "DomainPerformances",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerOptions_Questions_QuestionId",
                table: "AnswerOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_DomainPerformances_Domains_DomainId",
                table: "DomainPerformances");

            migrationBuilder.DropForeignKey(
                name: "FK_DomainPerformances_Exams_ExamId",
                table: "DomainPerformances");

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentageScore",
                table: "ExamAttempts",
                type: "numeric(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldComputedColumnSql: "CASE WHEN \"TotalQuestions\" = 0 THEN 0.0 ELSE (\"CorrectCount\"::numeric / \"TotalQuestions\"::numeric) * 100 END");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerOptions_Exams_ExamId",
                table: "AnswerOptions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DomainPerformances_Domains_DomainId",
                table: "DomainPerformances",
                column: "DomainId",
                principalTable: "Domains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DomainPerformances_Exams_ExamId",
                table: "DomainPerformances",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
