using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AccountProfilePreferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "display_name",
                schema: "workspace",
                table: "users",
                type: "text",
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "preferred_language",
                schema: "workspace",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "es"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "display_name", schema: "workspace", table: "users");

            migrationBuilder.DropColumn(
                name: "preferred_language",
                schema: "workspace",
                table: "users"
            );
        }
    }
}
