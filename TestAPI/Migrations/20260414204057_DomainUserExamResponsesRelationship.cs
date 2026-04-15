using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestAPI.Migrations
{
    /// <inheritdoc />
    public partial class DomainUserExamResponsesRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DomainId",
                table: "UserExamResponses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_UserExamResponses_DomainId",
                table: "UserExamResponses",
                column: "DomainId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserExamResponses_Domains_DomainId",
                table: "UserExamResponses",
                column: "DomainId",
                principalTable: "Domains",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserExamResponses_Domains_DomainId",
                table: "UserExamResponses");

            migrationBuilder.DropIndex(
                name: "IX_UserExamResponses_DomainId",
                table: "UserExamResponses");

            migrationBuilder.DropColumn(
                name: "DomainId",
                table: "UserExamResponses");
        }
    }
}
