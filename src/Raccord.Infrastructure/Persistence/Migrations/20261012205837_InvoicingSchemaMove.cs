using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InvoicingSchemaMove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_compliance_records_issued_invoices_invoice_id",
                schema: "compliance",
                table: "compliance_records"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_invoice_drafts_projects_project_id",
                table: "invoice_drafts"
            );

            migrationBuilder.DropIndex(
                name: "ix_invoice_drafts_project_id",
                table: "invoice_drafts"
            );

            migrationBuilder.EnsureSchema(name: "invoicing");

            migrationBuilder.RenameTable(
                name: "issued_invoices",
                newName: "issued_invoices",
                newSchema: "invoicing"
            );

            migrationBuilder.RenameTable(
                name: "invoice_number_sequences",
                newName: "invoice_number_sequences",
                newSchema: "invoicing"
            );

            migrationBuilder.RenameTable(
                name: "invoice_drafts",
                newName: "invoice_drafts",
                newSchema: "invoicing"
            );

            migrationBuilder.RenameTable(
                name: "invoice_draft_lines",
                newName: "invoice_draft_lines",
                newSchema: "invoicing"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "issued_invoices",
                schema: "invoicing",
                newName: "issued_invoices"
            );

            migrationBuilder.RenameTable(
                name: "invoice_number_sequences",
                schema: "invoicing",
                newName: "invoice_number_sequences"
            );

            migrationBuilder.RenameTable(
                name: "invoice_drafts",
                schema: "invoicing",
                newName: "invoice_drafts"
            );

            migrationBuilder.RenameTable(
                name: "invoice_draft_lines",
                schema: "invoicing",
                newName: "invoice_draft_lines"
            );

            migrationBuilder.CreateIndex(
                name: "ix_invoice_drafts_project_id",
                table: "invoice_drafts",
                column: "project_id"
            );

            migrationBuilder.AddForeignKey(
                name: "fk_compliance_records_issued_invoices_invoice_id",
                schema: "compliance",
                table: "compliance_records",
                column: "invoice_id",
                principalSchema: "invoicing",
                principalTable: "issued_invoices",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict
            );

            migrationBuilder.AddForeignKey(
                name: "fk_invoice_drafts_projects_project_id",
                table: "invoice_drafts",
                column: "project_id",
                principalSchema: "projects",
                principalTable: "projects",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict
            );
        }
    }
}
