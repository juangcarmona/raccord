using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ContactPersons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "contact_persons",
                schema: "contacts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    contact_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(
                        type: "character varying(200)",
                        maxLength: 200,
                        nullable: false
                    ),
                    email = table.Column<string>(
                        type: "character varying(320)",
                        maxLength: 320,
                        nullable: true
                    ),
                    phone = table.Column<string>(
                        type: "character varying(64)",
                        maxLength: 64,
                        nullable: true
                    ),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    is_administrative = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contact_persons", x => x.id);
                    table.ForeignKey(
                        name: "fk_contact_persons_customers_contact_id",
                        column: x => x.contact_id,
                        principalSchema: "contacts",
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_contact_persons_contact_administrative",
                schema: "contacts",
                table: "contact_persons",
                column: "contact_id",
                unique: true,
                filter: "is_administrative = TRUE"
            );

            migrationBuilder.CreateIndex(
                name: "ix_contact_persons_contact_default",
                schema: "contacts",
                table: "contact_persons",
                column: "contact_id",
                unique: true,
                filter: "is_default = TRUE"
            );

            migrationBuilder.CreateIndex(
                name: "ix_contact_persons_contact_name",
                schema: "contacts",
                table: "contact_persons",
                columns: new[] { "contact_id", "name" }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "contact_persons", schema: "contacts");
        }
    }
}
