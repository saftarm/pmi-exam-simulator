using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnusedSiteSettingsColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultExamDuration",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "NotifyOnExamComplete",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "NotifyOnNewUser",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "PassThreshold",
                table: "SiteSettings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DefaultExamDuration",
                table: "SiteSettings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "NotifyOnExamComplete",
                table: "SiteSettings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NotifyOnNewUser",
                table: "SiteSettings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PassThreshold",
                table: "SiteSettings",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
