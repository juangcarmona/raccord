using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EquipmentLifecycle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "disposed_at",
                schema: "equipment",
                table: "equipment_items",
                type: "timestamp with time zone",
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "disposition_reason",
                schema: "equipment",
                table: "equipment_items",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true
            );

            migrationBuilder.AddColumn<decimal>(
                name: "sale_amount",
                schema: "equipment",
                table: "equipment_items",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true
            );

            migrationBuilder.AddColumn<int>(
                name: "status",
                schema: "equipment",
                table: "equipment_items",
                type: "integer",
                nullable: false,
                defaultValue: 0
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "disposed_at",
                schema: "equipment",
                table: "equipment_items"
            );

            migrationBuilder.DropColumn(
                name: "disposition_reason",
                schema: "equipment",
                table: "equipment_items"
            );

            migrationBuilder.DropColumn(
                name: "sale_amount",
                schema: "equipment",
                table: "equipment_items"
            );

            migrationBuilder.DropColumn(
                name: "status",
                schema: "equipment",
                table: "equipment_items"
            );
        }
    }
}
