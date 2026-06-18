using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestAPI.Migrations
{
    /// <inheritdoc />
    public partial class AnswerOptionEntityChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DomainId",
                table: "AnswerOptions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOptions_DomainId",
                table: "AnswerOptions",
                column: "DomainId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerOptions_Domains_DomainId",
                table: "AnswerOptions",
                column: "DomainId",
                principalTable: "Domains",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerOptions_Domains_DomainId",
                table: "AnswerOptions");

            migrationBuilder.DropIndex(
                name: "IX_AnswerOptions_DomainId",
                table: "AnswerOptions");

            migrationBuilder.DropColumn(
                name: "DomainId",
                table: "AnswerOptions");
        }
    }
}
