using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixAnswerTypeDefaults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE \"Questions\" SET \"AnswerType\" = 1 WHERE \"AnswerType\" = 0;");
            migrationBuilder.Sql("UPDATE \"AnswerOptions\" SET \"AnswerType\" = 1 WHERE \"AnswerType\" = 0;");

            migrationBuilder.AlterColumn<int>(
                name: "AnswerType",
                table: "Questions",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "AnswerType",
                table: "AnswerOptions",
                type: "integer",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AnswerType",
                table: "Questions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "AnswerType",
                table: "AnswerOptions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
