using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIssuerIdentityReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "issued_invoice_count",
                schema: "workspace",
                table: "issuer_profiles",
                type: "integer",
                nullable: false,
                defaultValue: 0
            );

            migrationBuilder.Sql(
                """
                UPDATE workspace.issuer_profiles AS issuer
                SET issued_invoice_count = (
                    SELECT COUNT(*)::integer
                    FROM invoicing.issued_invoices AS invoice
                );
                """
            );

            migrationBuilder.CreateTable(
                name: "issuer_identity_changes",
                schema: "workspace",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    issuer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    old_legal_name = table.Column<string>(
                        type: "character varying(200)",
                        maxLength: 200,
                        nullable: false
                    ),
                    old_tax_id = table.Column<string>(
                        type: "character varying(32)",
                        maxLength: 32,
                        nullable: false
                    ),
                    proposed_legal_name = table.Column<string>(
                        type: "character varying(200)",
                        maxLength: 200,
                        nullable: false
                    ),
                    proposed_tax_id = table.Column<string>(
                        type: "character varying(32)",
                        maxLength: 32,
                        nullable: false
                    ),
                    reason = table.Column<string>(
                        type: "character varying(2000)",
                        maxLength: 2000,
                        nullable: false
                    ),
                    requested_by_id = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    requested_by_name = table.Column<string>(
                        type: "character varying(256)",
                        maxLength: 256,
                        nullable: false
                    ),
                    requested_at = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    status = table.Column<string>(
                        type: "character varying(16)",
                        maxLength: 16,
                        nullable: false
                    ),
                    decided_by_id = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: true
                    ),
                    decided_by_name = table.Column<string>(
                        type: "character varying(256)",
                        maxLength: 256,
                        nullable: true
                    ),
                    decided_at = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_issuer_identity_changes", x => x.id);
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_issuer_identity_changes_issuer_id",
                schema: "workspace",
                table: "issuer_identity_changes",
                column: "issuer_id",
                unique: true,
                filter: "\"status\" = 'pending'"
            );

            migrationBuilder.CreateIndex(
                name: "ix_issuer_identity_changes_status_requested_at",
                schema: "workspace",
                table: "issuer_identity_changes",
                columns: new[] { "status", "requested_at" }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "issuer_identity_changes", schema: "workspace");

            migrationBuilder.DropColumn(
                name: "issued_invoice_count",
                schema: "workspace",
                table: "issuer_profiles"
            );
        }
    }
}
