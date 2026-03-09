using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestAPI.Migrations
{
    /// <inheritdoc />
    public partial class ExamAttempts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamAttempt_Users_UserId",
                table: "ExamAttempt");

            migrationBuilder.DropForeignKey(
                name: "FK_UserExamResponse_ExamAttempt_ExamAttemptId",
                table: "UserExamResponse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExamAttempt",
                table: "ExamAttempt");

            migrationBuilder.RenameTable(
                name: "ExamAttempt",
                newName: "ExamAttempts");

            migrationBuilder.RenameIndex(
                name: "IX_ExamAttempt_UserId",
                table: "ExamAttempts",
                newName: "IX_ExamAttempts_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExamAttempts",
                table: "ExamAttempts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamAttempts_Users_UserId",
                table: "ExamAttempts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserExamResponse_ExamAttempts_ExamAttemptId",
                table: "UserExamResponse",
                column: "ExamAttemptId",
                principalTable: "ExamAttempts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamAttempts_Users_UserId",
                table: "ExamAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_UserExamResponse_ExamAttempts_ExamAttemptId",
                table: "UserExamResponse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExamAttempts",
                table: "ExamAttempts");

            migrationBuilder.RenameTable(
                name: "ExamAttempts",
                newName: "ExamAttempt");

            migrationBuilder.RenameIndex(
                name: "IX_ExamAttempts_UserId",
                table: "ExamAttempt",
                newName: "IX_ExamAttempt_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExamAttempt",
                table: "ExamAttempt",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamAttempt_Users_UserId",
                table: "ExamAttempt",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserExamResponse_ExamAttempt_ExamAttemptId",
                table: "UserExamResponse",
                column: "ExamAttemptId",
                principalTable: "ExamAttempt",
                principalColumn: "Id");
        }
    }
}
