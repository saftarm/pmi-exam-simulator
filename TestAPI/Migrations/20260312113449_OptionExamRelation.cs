using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestAPI.Migrations
{
    /// <inheritdoc />
    public partial class OptionExamRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "ExamId",
                table: "AnswerOptions",
                type: "integer",
                nullable: true
                );

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerOptions_Exams_ExamId",
                table: "AnswerOptions",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerOptions_Exams_ExamId",
                table: "AnswerOptions");

            migrationBuilder.AlterColumn<int>(
                name: "ExamId",
                table: "AnswerOptions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerOptions_Exams_ExamId",
                table: "AnswerOptions",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id");
        }
    }
}
