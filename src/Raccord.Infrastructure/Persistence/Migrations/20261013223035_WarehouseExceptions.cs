using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class WarehouseExceptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "exception_description",
                table: "packing_lines",
                type: "text",
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "exception_kind",
                table: "packing_lines",
                type: "text",
                nullable: true
            );

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "exception_reported_at",
                table: "packing_lines",
                type: "timestamp with time zone",
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "exception_reported_by",
                table: "packing_lines",
                type: "text",
                nullable: true
            );

            migrationBuilder.AddColumn<bool>(
                name: "has_exception",
                table: "packing_lines",
                type: "boolean",
                nullable: false,
                defaultValue: false
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "exception_description", table: "packing_lines");

            migrationBuilder.DropColumn(name: "exception_kind", table: "packing_lines");

            migrationBuilder.DropColumn(name: "exception_reported_at", table: "packing_lines");

            migrationBuilder.DropColumn(name: "exception_reported_by", table: "packing_lines");

            migrationBuilder.DropColumn(name: "has_exception", table: "packing_lines");
        }
    }
}
