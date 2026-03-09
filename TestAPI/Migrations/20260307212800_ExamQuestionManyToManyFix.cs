using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TestAPI.Migrations
{
    /// <inheritdoc />
    public partial class ExamQuestionManyToManyFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Exams_ExamId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamsQuestions_Exams_ExamsId",
                table: "ExamsQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamsQuestions_Questions_QuestionsId",
                table: "ExamsQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExamsQuestions",
                table: "ExamsQuestions");

            migrationBuilder.DropIndex(
                name: "IX_Exams_ExamId",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "ExamId",
                table: "Exams");

            migrationBuilder.RenameColumn(
                name: "QuestionsId",
                table: "ExamsQuestions",
                newName: "QuestionId");

            migrationBuilder.RenameColumn(
                name: "ExamsId",
                table: "ExamsQuestions",
                newName: "ExamId");

            migrationBuilder.RenameIndex(
                name: "IX_ExamsQuestions_QuestionsId",
                table: "ExamsQuestions",
                newName: "IX_ExamsQuestions_QuestionId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ExamsQuestions",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExamsQuestions",
                table: "ExamsQuestions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ExamQuestion",
                columns: table => new
                {
                    ExamsId = table.Column<int>(type: "integer", nullable: false),
                    QuestionsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamQuestion", x => new { x.ExamsId, x.QuestionsId });
                    table.ForeignKey(
                        name: "FK_ExamQuestion_Exams_ExamsId",
                        column: x => x.ExamsId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamQuestion_Questions_QuestionsId",
                        column: x => x.QuestionsId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamsQuestions_ExamId",
                table: "ExamsQuestions",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamQuestion_QuestionsId",
                table: "ExamQuestion",
                column: "QuestionsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamsQuestions_Exams_ExamId",
                table: "ExamsQuestions",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamsQuestions_Questions_QuestionId",
                table: "ExamsQuestions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamsQuestions_Exams_ExamId",
                table: "ExamsQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamsQuestions_Questions_QuestionId",
                table: "ExamsQuestions");

            migrationBuilder.DropTable(
                name: "ExamQuestion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExamsQuestions",
                table: "ExamsQuestions");

            migrationBuilder.DropIndex(
                name: "IX_ExamsQuestions_ExamId",
                table: "ExamsQuestions");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ExamsQuestions");

            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "ExamsQuestions",
                newName: "QuestionsId");

            migrationBuilder.RenameColumn(
                name: "ExamId",
                table: "ExamsQuestions",
                newName: "ExamsId");

            migrationBuilder.RenameIndex(
                name: "IX_ExamsQuestions_QuestionId",
                table: "ExamsQuestions",
                newName: "IX_ExamsQuestions_QuestionsId");

            migrationBuilder.AddColumn<int>(
                name: "ExamId",
                table: "Exams",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExamsQuestions",
                table: "ExamsQuestions",
                columns: new[] { "ExamsId", "QuestionsId" });

            migrationBuilder.CreateIndex(
                name: "IX_Exams_ExamId",
                table: "Exams",
                column: "ExamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Exams_ExamId",
                table: "Exams",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamsQuestions_Exams_ExamsId",
                table: "ExamsQuestions",
                column: "ExamsId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamsQuestions_Questions_QuestionsId",
                table: "ExamsQuestions",
                column: "QuestionsId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
