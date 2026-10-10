using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialSlice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "audit_events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    occurred_at = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    actor = table.Column<string>(
                        type: "character varying(200)",
                        maxLength: 200,
                        nullable: false
                    ),
                    action = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    entity_type = table.Column<string>(
                        type: "character varying(50)",
                        maxLength: 50,
                        nullable: false
                    ),
                    entity_id = table.Column<Guid>(type: "uuid", nullable: false),
                    details = table.Column<string>(type: "jsonb", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_audit_events", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    legal_name = table.Column<string>(
                        type: "character varying(200)",
                        maxLength: 200,
                        nullable: false
                    ),
                    tax_id = table.Column<string>(
                        type: "character varying(32)",
                        maxLength: 32,
                        nullable: true
                    ),
                    email = table.Column<string>(
                        type: "character varying(320)",
                        maxLength: 320,
                        nullable: true
                    ),
                    city = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: true
                    ),
                    country_code = table.Column<string>(
                        type: "character varying(2)",
                        maxLength: 2,
                        nullable: true
                    ),
                    postal_code = table.Column<string>(
                        type: "character varying(20)",
                        maxLength: 20,
                        nullable: true
                    ),
                    province = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: true
                    ),
                    street = table.Column<string>(
                        type: "character varying(200)",
                        maxLength: 200,
                        nullable: true
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customers", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "invoice_series",
                columns: table => new
                {
                    code = table.Column<string>(
                        type: "character varying(10)",
                        maxLength: 10,
                        nullable: false
                    ),
                    description = table.Column<string>(
                        type: "character varying(200)",
                        maxLength: 200,
                        nullable: false
                    ),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoice_series", x => x.code);
                }
            );

            migrationBuilder.CreateTable(
                name: "issued_invoices",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    series_code = table.Column<string>(
                        type: "character varying(10)",
                        maxLength: 10,
                        nullable: false
                    ),
                    year = table.Column<int>(type: "integer", nullable: false),
                    sequence = table.Column<int>(type: "integer", nullable: false),
                    number = table.Column<string>(
                        type: "character varying(32)",
                        maxLength: 32,
                        nullable: false
                    ),
                    issue_date = table.Column<DateOnly>(type: "date", nullable: false),
                    issued_at = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    draft_id = table.Column<Guid>(type: "uuid", nullable: false),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    recipient_name = table.Column<string>(
                        type: "character varying(200)",
                        maxLength: 200,
                        nullable: false
                    ),
                    recipient_tax_id = table.Column<string>(
                        type: "character varying(32)",
                        maxLength: 32,
                        nullable: true
                    ),
                    total = table.Column<decimal>(
                        type: "numeric(18,2)",
                        precision: 18,
                        scale: 2,
                        nullable: false
                    ),
                    currency = table.Column<string>(
                        type: "character varying(3)",
                        maxLength: 3,
                        nullable: false
                    ),
                    representation = table.Column<string>(type: "text", nullable: false),
                    content_hash = table.Column<string>(
                        type: "character(64)",
                        fixedLength: true,
                        maxLength: 64,
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_issued_invoices", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "issuer_profiles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    legal_name = table.Column<string>(
                        type: "character varying(200)",
                        maxLength: 200,
                        nullable: false
                    ),
                    tax_id = table.Column<string>(
                        type: "character varying(32)",
                        maxLength: 32,
                        nullable: false
                    ),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    city = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: true
                    ),
                    country_code = table.Column<string>(
                        type: "character varying(2)",
                        maxLength: 2,
                        nullable: true
                    ),
                    postal_code = table.Column<string>(
                        type: "character varying(20)",
                        maxLength: 20,
                        nullable: true
                    ),
                    province = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: true
                    ),
                    street = table.Column<string>(
                        type: "character varying(200)",
                        maxLength: 200,
                        nullable: true
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_issuer_profiles", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "projects",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(
                        type: "character varying(200)",
                        maxLength: 200,
                        nullable: false
                    ),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_projects", x => x.id);
                    table.ForeignKey(
                        name: "fk_projects_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "invoice_number_sequences",
                columns: table => new
                {
                    series_code = table.Column<string>(
                        type: "character varying(10)",
                        maxLength: 10,
                        nullable: false
                    ),
                    year = table.Column<int>(type: "integer", nullable: false),
                    last_number = table.Column<int>(type: "integer", nullable: false),
                    last_issue_date = table.Column<DateOnly>(type: "date", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "pk_invoice_number_sequences",
                        x => new { x.series_code, x.year }
                    );
                    table.ForeignKey(
                        name: "fk_invoice_number_sequences_invoice_series_series_code",
                        column: x => x.series_code,
                        principalTable: "invoice_series",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "compliance_records",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    invoice_id = table.Column<Guid>(type: "uuid", nullable: false),
                    invoice_number = table.Column<string>(
                        type: "character varying(32)",
                        maxLength: 32,
                        nullable: false
                    ),
                    content_hash = table.Column<string>(
                        type: "character(64)",
                        fixedLength: true,
                        maxLength: 64,
                        nullable: false
                    ),
                    kind = table.Column<string>(
                        type: "character varying(30)",
                        maxLength: 30,
                        nullable: false
                    ),
                    status = table.Column<string>(
                        type: "character varying(30)",
                        maxLength: 30,
                        nullable: false
                    ),
                    created_at = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    attempts = table.Column<int>(type: "integer", nullable: false),
                    last_attempt_at = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    last_error = table.Column<string>(
                        type: "character varying(2000)",
                        maxLength: 2000,
                        nullable: true
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_compliance_records", x => x.id);
                    table.ForeignKey(
                        name: "fk_compliance_records_issued_invoices_invoice_id",
                        column: x => x.invoice_id,
                        principalTable: "issued_invoices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "billable_lines",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    kind = table.Column<string>(
                        type: "character varying(20)",
                        maxLength: 20,
                        nullable: false
                    ),
                    description = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: false
                    ),
                    quantity = table.Column<decimal>(
                        type: "numeric(18,4)",
                        precision: 18,
                        scale: 4,
                        nullable: false
                    ),
                    unit_price = table.Column<decimal>(
                        type: "numeric(18,4)",
                        precision: 18,
                        scale: 4,
                        nullable: false
                    ),
                    discount_percent = table.Column<decimal>(
                        type: "numeric(5,2)",
                        precision: 5,
                        scale: 2,
                        nullable: false
                    ),
                    vat_percentage = table.Column<decimal>(
                        type: "numeric(5,2)",
                        precision: 5,
                        scale: 2,
                        nullable: false
                    ),
                    currency_code = table.Column<string>(
                        type: "character varying(3)",
                        maxLength: 3,
                        nullable: false
                    ),
                    state = table.Column<string>(
                        type: "character varying(20)",
                        maxLength: 20,
                        nullable: false
                    ),
                    draft_id = table.Column<Guid>(type: "uuid", nullable: true),
                    invoice_id = table.Column<Guid>(type: "uuid", nullable: true),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_billable_lines", x => x.id);
                    table.ForeignKey(
                        name: "fk_billable_lines_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "invoice_drafts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    operation_date = table.Column<DateOnly>(type: "date", nullable: false),
                    series_code = table.Column<string>(
                        type: "character varying(10)",
                        maxLength: 10,
                        nullable: false
                    ),
                    created_at = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    status = table.Column<string>(
                        type: "character varying(20)",
                        maxLength: 20,
                        nullable: false
                    ),
                    issued_invoice_id = table.Column<Guid>(type: "uuid", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoice_drafts", x => x.id);
                    table.ForeignKey(
                        name: "fk_invoice_drafts_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "invoice_draft_lines",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    position = table.Column<int>(type: "integer", nullable: false),
                    source_billable_line_id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: false
                    ),
                    quantity = table.Column<decimal>(
                        type: "numeric(18,4)",
                        precision: 18,
                        scale: 4,
                        nullable: false
                    ),
                    unit_price = table.Column<decimal>(
                        type: "numeric(18,4)",
                        precision: 18,
                        scale: 4,
                        nullable: false
                    ),
                    discount_percent = table.Column<decimal>(
                        type: "numeric(5,2)",
                        precision: 5,
                        scale: 2,
                        nullable: false
                    ),
                    vat_percentage = table.Column<decimal>(
                        type: "numeric(5,2)",
                        precision: 5,
                        scale: 2,
                        nullable: false
                    ),
                    currency_code = table.Column<string>(
                        type: "character varying(3)",
                        maxLength: 3,
                        nullable: false
                    ),
                    draft_id = table.Column<Guid>(type: "uuid", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoice_draft_lines", x => x.id);
                    table.ForeignKey(
                        name: "fk_invoice_draft_lines_invoice_drafts_draft_id",
                        column: x => x.draft_id,
                        principalTable: "invoice_drafts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_audit_events_entity_id_occurred_at",
                table: "audit_events",
                columns: new[] { "entity_id", "occurred_at" }
            );

            migrationBuilder.CreateIndex(
                name: "ix_billable_lines_draft_id",
                table: "billable_lines",
                column: "draft_id"
            );

            migrationBuilder.CreateIndex(
                name: "ix_billable_lines_project_id",
                table: "billable_lines",
                column: "project_id"
            );

            migrationBuilder.CreateIndex(
                name: "ix_compliance_records_invoice_id",
                table: "compliance_records",
                column: "invoice_id",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_compliance_records_status",
                table: "compliance_records",
                column: "status"
            );

            migrationBuilder.CreateIndex(
                name: "ix_customers_tax_id",
                table: "customers",
                column: "tax_id"
            );

            migrationBuilder.CreateIndex(
                name: "ix_invoice_draft_lines_draft_id",
                table: "invoice_draft_lines",
                column: "draft_id"
            );

            migrationBuilder.CreateIndex(
                name: "ix_invoice_drafts_project_id",
                table: "invoice_drafts",
                column: "project_id"
            );

            migrationBuilder.CreateIndex(
                name: "ix_invoice_drafts_status",
                table: "invoice_drafts",
                column: "status"
            );

            migrationBuilder.CreateIndex(
                name: "ix_issued_invoices_draft_id",
                table: "issued_invoices",
                column: "draft_id",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_issued_invoices_issue_date",
                table: "issued_invoices",
                column: "issue_date"
            );

            migrationBuilder.CreateIndex(
                name: "ix_issued_invoices_number",
                table: "issued_invoices",
                column: "number",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_issued_invoices_series_code_year_sequence",
                table: "issued_invoices",
                columns: new[] { "series_code", "year", "sequence" },
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_projects_customer_id",
                table: "projects",
                column: "customer_id"
            );

            // Issued invoices and audit events are append-only; compliance records may change status but never disappear.
            migrationBuilder.Sql(
                """
                CREATE FUNCTION raccord_reject_mutation() RETURNS trigger LANGUAGE plpgsql AS $$
                BEGIN
                    RAISE EXCEPTION '% on % is not allowed: the table is append-only', TG_OP, TG_TABLE_NAME
                        USING ERRCODE = 'integrity_constraint_violation';
                END;
                $$;

                CREATE TRIGGER issued_invoices_append_only BEFORE UPDATE OR DELETE ON issued_invoices
                    FOR EACH ROW EXECUTE FUNCTION raccord_reject_mutation();
                CREATE TRIGGER issued_invoices_no_truncate BEFORE TRUNCATE ON issued_invoices
                    FOR EACH STATEMENT EXECUTE FUNCTION raccord_reject_mutation();
                CREATE TRIGGER audit_events_append_only BEFORE UPDATE OR DELETE ON audit_events
                    FOR EACH ROW EXECUTE FUNCTION raccord_reject_mutation();
                CREATE TRIGGER audit_events_no_truncate BEFORE TRUNCATE ON audit_events
                    FOR EACH STATEMENT EXECUTE FUNCTION raccord_reject_mutation();
                CREATE TRIGGER compliance_records_no_delete BEFORE DELETE ON compliance_records
                    FOR EACH ROW EXECUTE FUNCTION raccord_reject_mutation();
                CREATE TRIGGER compliance_records_no_truncate BEFORE TRUNCATE ON compliance_records
                    FOR EACH STATEMENT EXECUTE FUNCTION raccord_reject_mutation();
                """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DROP TRIGGER IF EXISTS compliance_records_no_truncate ON compliance_records;
                DROP TRIGGER IF EXISTS compliance_records_no_delete ON compliance_records;
                DROP TRIGGER IF EXISTS audit_events_no_truncate ON audit_events;
                DROP TRIGGER IF EXISTS audit_events_append_only ON audit_events;
                DROP TRIGGER IF EXISTS issued_invoices_no_truncate ON issued_invoices;
                DROP TRIGGER IF EXISTS issued_invoices_append_only ON issued_invoices;
                DROP FUNCTION IF EXISTS raccord_reject_mutation();
                """
            );

            migrationBuilder.DropTable(name: "audit_events");

            migrationBuilder.DropTable(name: "billable_lines");

            migrationBuilder.DropTable(name: "compliance_records");

            migrationBuilder.DropTable(name: "invoice_draft_lines");

            migrationBuilder.DropTable(name: "invoice_number_sequences");

            migrationBuilder.DropTable(name: "issuer_profiles");

            migrationBuilder.DropTable(name: "issued_invoices");

            migrationBuilder.DropTable(name: "invoice_drafts");

            migrationBuilder.DropTable(name: "invoice_series");

            migrationBuilder.DropTable(name: "projects");

            migrationBuilder.DropTable(name: "customers");
        }
    }
}
