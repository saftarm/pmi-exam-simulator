using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestAPI.Migrations
{
    /// <inheritdoc />
    public partial class QuestionDomainToNoAction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Domains_DomainId",
                table: "Questions");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Domains_DomainId",
                table: "Questions",
                column: "DomainId",
                principalTable: "Domains",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Domains_DomainId",
                table: "Questions");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Domains_DomainId",
                table: "Questions",
                column: "DomainId",
                principalTable: "Domains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
