using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EquipmentItemDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "accessory_ids",
                schema: "equipment",
                table: "items",
                type: "jsonb",
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.AddColumn<string>(
                name: "alternative_ids",
                schema: "equipment",
                table: "items",
                type: "jsonb",
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.AddColumn<string>(
                name: "details",
                schema: "equipment",
                table: "items",
                type: "character varying(5000)",
                maxLength: 5000,
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "image_key",
                schema: "equipment",
                table: "items",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "accessory_ids", schema: "equipment", table: "items");

            migrationBuilder.DropColumn(
                name: "alternative_ids",
                schema: "equipment",
                table: "items"
            );

            migrationBuilder.DropColumn(name: "details", schema: "equipment", table: "items");

            migrationBuilder.DropColumn(name: "image_key", schema: "equipment", table: "items");
        }
    }
}
