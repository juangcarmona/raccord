using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class WarehouseReceiving : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_received",
                table: "packing_lines",
                type: "boolean",
                nullable: false,
                defaultValue: false
            );

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "received_at",
                table: "packing_lines",
                type: "timestamp with time zone",
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "received_by",
                table: "packing_lines",
                type: "text",
                nullable: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "is_received", table: "packing_lines");

            migrationBuilder.DropColumn(name: "received_at", table: "packing_lines");

            migrationBuilder.DropColumn(name: "received_by", table: "packing_lines");
        }
    }
}
