using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class WarehouseBookOut : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "booked_out_at",
                table: "packing_lines",
                type: "timestamp with time zone",
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "booked_out_by",
                table: "packing_lines",
                type: "text",
                nullable: true
            );

            migrationBuilder.AddColumn<bool>(
                name: "is_booked_out",
                table: "packing_lines",
                type: "boolean",
                nullable: false,
                defaultValue: false
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "booked_out_at", table: "packing_lines");

            migrationBuilder.DropColumn(name: "booked_out_by", table: "packing_lines");

            migrationBuilder.DropColumn(name: "is_booked_out", table: "packing_lines");
        }
    }
}
