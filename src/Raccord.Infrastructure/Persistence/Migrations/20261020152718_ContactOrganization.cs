using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ContactOrganization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "folder_id",
                schema: "contacts",
                table: "customers",
                type: "uuid",
                nullable: true
            );

            migrationBuilder.CreateTable(
                name: "contact_folders",
                schema: "contacts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: true),
                    position = table.Column<int>(type: "integer", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contact_folders", x => x.id);
                    table.ForeignKey(
                        name: "fk_contact_folders_contact_folders_parent_id",
                        column: x => x.parent_id,
                        principalSchema: "contacts",
                        principalTable: "contact_folders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "contact_tags",
                schema: "contacts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    name_key = table.Column<string>(
                        type: "character varying(64)",
                        maxLength: 64,
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contact_tags", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "contact_tag_assignments",
                schema: "contacts",
                columns: table => new
                {
                    contact_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tag_id = table.Column<Guid>(type: "uuid", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "pk_contact_tag_assignments",
                        x => new { x.contact_id, x.tag_id }
                    );
                    table.ForeignKey(
                        name: "fk_contact_tag_assignments_contact_tags_tag_id",
                        column: x => x.tag_id,
                        principalSchema: "contacts",
                        principalTable: "contact_tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict
                    );
                    table.ForeignKey(
                        name: "fk_contact_tag_assignments_customers_contact_id",
                        column: x => x.contact_id,
                        principalSchema: "contacts",
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_customers_folder_id",
                schema: "contacts",
                table: "customers",
                column: "folder_id"
            );

            migrationBuilder.CreateIndex(
                name: "ix_contact_folders_parent_id_position",
                schema: "contacts",
                table: "contact_folders",
                columns: new[] { "parent_id", "position" }
            );

            migrationBuilder.CreateIndex(
                name: "ix_contact_tag_assignments_tag_id",
                schema: "contacts",
                table: "contact_tag_assignments",
                column: "tag_id"
            );

            migrationBuilder.CreateIndex(
                name: "ix_contact_tags_name_key",
                schema: "contacts",
                table: "contact_tags",
                column: "name_key",
                unique: true
            );

            migrationBuilder.AddForeignKey(
                name: "fk_customers_contact_folders_folder_id",
                schema: "contacts",
                table: "customers",
                column: "folder_id",
                principalSchema: "contacts",
                principalTable: "contact_folders",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_customers_contact_folders_folder_id",
                schema: "contacts",
                table: "customers"
            );

            migrationBuilder.DropTable(name: "contact_folders", schema: "contacts");

            migrationBuilder.DropTable(name: "contact_tag_assignments", schema: "contacts");

            migrationBuilder.DropTable(name: "contact_tags", schema: "contacts");

            migrationBuilder.DropIndex(
                name: "ix_customers_folder_id",
                schema: "contacts",
                table: "customers"
            );

            migrationBuilder.DropColumn(name: "folder_id", schema: "contacts", table: "customers");
        }
    }
}
