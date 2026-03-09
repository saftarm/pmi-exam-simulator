using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestAPI.Migrations
{
    /// <inheritdoc />
    public partial class ResponseAttemptRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserExamResponse_ExamAttempts_ExamAttemptId",
                table: "UserExamResponse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserExamResponse",
                table: "UserExamResponse");

            migrationBuilder.RenameTable(
                name: "UserExamResponse",
                newName: "UserExamResponses");

            migrationBuilder.RenameIndex(
                name: "IX_UserExamResponse_ExamAttemptId",
                table: "UserExamResponses",
                newName: "IX_UserExamResponses_ExamAttemptId");

            migrationBuilder.AlterColumn<int>(
                name: "ExamAttemptId",
                table: "UserExamResponses",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserExamResponses",
                table: "UserExamResponses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserExamResponses_ExamAttempts_ExamAttemptId",
                table: "UserExamResponses",
                column: "ExamAttemptId",
                principalTable: "ExamAttempts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserExamResponses_ExamAttempts_ExamAttemptId",
                table: "UserExamResponses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserExamResponses",
                table: "UserExamResponses");

            migrationBuilder.RenameTable(
                name: "UserExamResponses",
                newName: "UserExamResponse");

            migrationBuilder.RenameIndex(
                name: "IX_UserExamResponses_ExamAttemptId",
                table: "UserExamResponse",
                newName: "IX_UserExamResponse_ExamAttemptId");

            migrationBuilder.AlterColumn<int>(
                name: "ExamAttemptId",
                table: "UserExamResponse",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserExamResponse",
                table: "UserExamResponse",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserExamResponse_ExamAttempts_ExamAttemptId",
                table: "UserExamResponse",
                column: "ExamAttemptId",
                principalTable: "ExamAttempts",
                principalColumn: "Id");
        }
    }
}
