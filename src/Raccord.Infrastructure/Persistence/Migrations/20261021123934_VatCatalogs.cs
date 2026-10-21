using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class VatCatalogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "vat_class_id",
                schema: "invoicing",
                table: "invoice_draft_lines",
                type: "uuid",
                nullable: true
            );

            migrationBuilder.AddColumn<Guid>(
                name: "vat_scheme_id",
                schema: "invoicing",
                table: "invoice_draft_lines",
                type: "uuid",
                nullable: true
            );

            migrationBuilder.AddColumn<Guid>(
                name: "vat_class_id",
                schema: "projects",
                table: "billable_lines",
                type: "uuid",
                nullable: true
            );

            migrationBuilder.AddColumn<Guid>(
                name: "vat_scheme_id",
                schema: "projects",
                table: "billable_lines",
                type: "uuid",
                nullable: true
            );

            migrationBuilder.CreateTable(
                name: "vat_classes",
                schema: "invoicing",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vat_classes", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "vat_schemes",
                schema: "invoicing",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    kind = table.Column<string>(
                        type: "character varying(30)",
                        maxLength: 30,
                        nullable: false
                    ),
                    fixed_percentage = table.Column<decimal>(
                        type: "numeric(5,2)",
                        precision: 5,
                        scale: 2,
                        nullable: true
                    ),
                    is_used = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    territory = table.Column<string>(
                        type: "character varying(2)",
                        maxLength: 2,
                        nullable: true
                    ),
                    source_url = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: true
                    ),
                    verified_on = table.Column<DateOnly>(type: "date", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vat_schemes", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "vat_scheme_class_rates",
                schema: "invoicing",
                columns: table => new
                {
                    vat_class_id = table.Column<Guid>(type: "uuid", nullable: false),
                    vat_scheme_id = table.Column<Guid>(type: "uuid", nullable: false),
                    percentage = table.Column<decimal>(
                        type: "numeric(5,2)",
                        precision: 5,
                        scale: 2,
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "pk_vat_scheme_class_rates",
                        x => new { x.vat_scheme_id, x.vat_class_id }
                    );
                    table.ForeignKey(
                        name: "fk_vat_scheme_class_rates_vat_classes_vat_class_id",
                        column: x => x.vat_class_id,
                        principalSchema: "invoicing",
                        principalTable: "vat_classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "fk_vat_scheme_class_rates_vat_schemes_vat_scheme_id",
                        column: x => x.vat_scheme_id,
                        principalSchema: "invoicing",
                        principalTable: "vat_schemes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_vat_classes_name",
                schema: "invoicing",
                table: "vat_classes",
                column: "name",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_vat_scheme_class_rates_vat_class_id",
                schema: "invoicing",
                table: "vat_scheme_class_rates",
                column: "vat_class_id"
            );

            migrationBuilder.CreateIndex(
                name: "ix_vat_schemes_is_default",
                schema: "invoicing",
                table: "vat_schemes",
                column: "is_default",
                unique: true,
                filter: "is_default"
            );

            migrationBuilder.CreateIndex(
                name: "ix_vat_schemes_name",
                schema: "invoicing",
                table: "vat_schemes",
                column: "name",
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "vat_scheme_class_rates", schema: "invoicing");

            migrationBuilder.DropTable(name: "vat_classes", schema: "invoicing");

            migrationBuilder.DropTable(name: "vat_schemes", schema: "invoicing");

            migrationBuilder.DropColumn(
                name: "vat_class_id",
                schema: "invoicing",
                table: "invoice_draft_lines"
            );

            migrationBuilder.DropColumn(
                name: "vat_scheme_id",
                schema: "invoicing",
                table: "invoice_draft_lines"
            );

            migrationBuilder.DropColumn(
                name: "vat_class_id",
                schema: "projects",
                table: "billable_lines"
            );

            migrationBuilder.DropColumn(
                name: "vat_scheme_id",
                schema: "projects",
                table: "billable_lines"
            );
        }
    }
}
