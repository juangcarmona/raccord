using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ContactTaxonomy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "folder",
                schema: "contacts",
                table: "customers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "tags",
                schema: "contacts",
                table: "customers",
                type: "jsonb",
                nullable: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "folder", schema: "contacts", table: "customers");

            migrationBuilder.DropColumn(name: "tags", schema: "contacts", table: "customers");
        }
    }
}
