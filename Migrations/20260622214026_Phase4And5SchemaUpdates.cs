using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestAPI.Migrations
{
    /// <inheritdoc />
    public partial class Phase4And5SchemaUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfExams",
                table: "Categories");

            migrationBuilder.AddColumn<int>(
                name: "AnswerType",
                table: "Questions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AnswerType",
                table: "AnswerOptions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnswerType",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "AnswerType",
                table: "AnswerOptions");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfExams",
                table: "Categories",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
