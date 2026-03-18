using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TestAPI.Migrations
{
    /// <inheritdoc />
    public partial class DomainEntityFieldsNullabilityFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.AlterColumn<string>(
    name: "Description",
    table: "Domains",
    type: "text",
    nullable: true,
    oldClrType: typeof(string),
    oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}
