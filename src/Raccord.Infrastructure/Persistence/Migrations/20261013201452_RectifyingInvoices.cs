using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RectifyingInvoices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "original_invoice_id",
                table: "invoice_drafts",
                type: "uuid",
                nullable: true
            );

            migrationBuilder.AddColumn<int>(
                name: "rectification_kind",
                table: "invoice_drafts",
                type: "integer",
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "rectification_reason",
                table: "invoice_drafts",
                type: "text",
                nullable: true
            );

            migrationBuilder.AddColumn<bool>(
                name: "is_adjustment",
                table: "invoice_draft_lines",
                type: "boolean",
                nullable: false,
                defaultValue: false
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "original_invoice_id", table: "invoice_drafts");

            migrationBuilder.DropColumn(name: "rectification_kind", table: "invoice_drafts");

            migrationBuilder.DropColumn(name: "rectification_reason", table: "invoice_drafts");

            migrationBuilder.DropColumn(name: "is_adjustment", table: "invoice_draft_lines");
        }
    }
}
