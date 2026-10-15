using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class WarehouseExceptionsAndDispositions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "disposition",
                schema: "projects",
                table: "packing_lines",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.AddColumn<string>(
                name: "exception_description",
                schema: "projects",
                table: "packing_lines",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "exception_kind",
                schema: "projects",
                table: "packing_lines",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true
            );

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "exception_reported_at",
                schema: "projects",
                table: "packing_lines",
                type: "timestamp with time zone",
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "exception_reported_by",
                schema: "projects",
                table: "packing_lines",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "disposition",
                schema: "projects",
                table: "packing_lines"
            );

            migrationBuilder.DropColumn(
                name: "exception_description",
                schema: "projects",
                table: "packing_lines"
            );

            migrationBuilder.DropColumn(
                name: "exception_kind",
                schema: "projects",
                table: "packing_lines"
            );

            migrationBuilder.DropColumn(
                name: "exception_reported_at",
                schema: "projects",
                table: "packing_lines"
            );

            migrationBuilder.DropColumn(
                name: "exception_reported_by",
                schema: "projects",
                table: "packing_lines"
            );
        }
    }
}
