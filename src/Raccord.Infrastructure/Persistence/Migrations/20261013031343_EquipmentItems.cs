using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EquipmentItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: "equipment");

            migrationBuilder.CreateTable(
                name: "items",
                schema: "equipment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    name = table.Column<string>(
                        type: "character varying(200)",
                        maxLength: 200,
                        nullable: false
                    ),
                    type = table.Column<int>(type: "integer", nullable: false),
                    stock_quantity = table.Column<int>(type: "integer", nullable: false),
                    serial_numbers = table.Column<string>(type: "jsonb", nullable: false),
                    is_planned = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_items", x => x.id);
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_items_code",
                schema: "equipment",
                table: "items",
                column: "code",
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "items", schema: "equipment");
        }
    }
}
