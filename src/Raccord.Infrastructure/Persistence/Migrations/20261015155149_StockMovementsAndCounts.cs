using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class StockMovementsAndCounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "stock_counts",
                schema: "equipment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    equipment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    counted_quantity = table.Column<int>(type: "integer", nullable: false),
                    previous_quantity = table.Column<int>(type: "integer", nullable: false),
                    difference = table.Column<int>(type: "integer", nullable: false),
                    reason = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: false
                    ),
                    counted_at = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_stock_counts", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "stock_movements",
                schema: "equipment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    equipment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    direction = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    scheduled_date = table.Column<DateOnly>(type: "date", nullable: false),
                    reference = table.Column<string>(
                        type: "character varying(200)",
                        maxLength: 200,
                        nullable: true
                    ),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_stock_movements", x => x.id);
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_stock_counts_equipment_id_counted_at",
                schema: "equipment",
                table: "stock_counts",
                columns: new[] { "equipment_id", "counted_at" }
            );

            migrationBuilder.CreateIndex(
                name: "ix_stock_movements_equipment_id_scheduled_date",
                schema: "equipment",
                table: "stock_movements",
                columns: new[] { "equipment_id", "scheduled_date" }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "stock_counts", schema: "equipment");

            migrationBuilder.DropTable(name: "stock_movements", schema: "equipment");
        }
    }
}
