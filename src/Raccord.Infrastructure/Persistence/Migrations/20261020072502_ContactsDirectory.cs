using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ContactsDirectory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "contact_number",
                schema: "contacts",
                table: "customers",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "type",
                schema: "contacts",
                table: "customers",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true
            );

            migrationBuilder.CreateTable(
                name: "contact_roles",
                schema: "contacts",
                columns: table => new
                {
                    contact_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role = table.Column<string>(
                        type: "character varying(32)",
                        maxLength: 32,
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contact_roles", x => new { x.contact_id, x.role });
                    table.ForeignKey(
                        name: "fk_contact_roles_customers_contact_id",
                        column: x => x.contact_id,
                        principalSchema: "contacts",
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_customers_contact_number",
                schema: "contacts",
                table: "customers",
                column: "contact_number",
                unique: true
            );

            migrationBuilder.Sql(
                """
                WITH ranked AS (
                    SELECT id, row_number() OVER (ORDER BY id) AS position
                    FROM contacts.customers
                )
                UPDATE contacts.customers AS customer
                SET contact_number = (10000 + ranked.position)::text,
                    type = 'Company'
                FROM ranked
                WHERE ranked.id = customer.id;

                INSERT INTO contacts.contact_roles (contact_id, role)
                SELECT id, 'Customer' FROM contacts.customers;

                CREATE SEQUENCE contacts.contact_number_seq START WITH 10001;
                SELECT setval(
                    'contacts.contact_number_seq',
                    COALESCE((SELECT MAX(contact_number::bigint) FROM contacts.customers), 10000),
                    true
                );

                ALTER TABLE contacts.customers ALTER COLUMN type SET NOT NULL;
                """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP SEQUENCE IF EXISTS contacts.contact_number_seq;");
            migrationBuilder.DropTable(name: "contact_roles", schema: "contacts");

            migrationBuilder.DropIndex(
                name: "ix_customers_contact_number",
                schema: "contacts",
                table: "customers"
            );

            migrationBuilder.DropColumn(
                name: "contact_number",
                schema: "contacts",
                table: "customers"
            );

            migrationBuilder.DropColumn(name: "type", schema: "contacts", table: "customers");
        }
    }
}
