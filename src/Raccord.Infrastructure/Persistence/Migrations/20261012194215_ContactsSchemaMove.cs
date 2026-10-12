using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ContactsSchemaMove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_projects_customers_customer_id",
                table: "projects"
            );

            migrationBuilder.DropIndex(name: "ix_projects_customer_id", table: "projects");

            migrationBuilder.EnsureSchema(name: "contacts");

            migrationBuilder.RenameTable(
                name: "customers",
                newName: "customers",
                newSchema: "contacts"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "customers",
                schema: "contacts",
                newName: "customers"
            );

            migrationBuilder.CreateIndex(
                name: "ix_projects_customer_id",
                table: "projects",
                column: "customer_id"
            );

            migrationBuilder.AddForeignKey(
                name: "fk_projects_customers_customer_id",
                table: "projects",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict
            );
        }
    }
}
