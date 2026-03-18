using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestAPI.Migrations
{
    /// <inheritdoc />
    public partial class QuestionDomainRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DomainId",
                table: "Questions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_DomainId",
                table: "Questions",
                column: "DomainId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Domains_DomainId",
                table: "Questions",
                column: "DomainId",
                principalTable: "Domains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Domains_DomainId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_DomainId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "DomainId",
                table: "Questions");
        }
    }
}
