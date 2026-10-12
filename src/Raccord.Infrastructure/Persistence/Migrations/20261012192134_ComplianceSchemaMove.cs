using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ComplianceSchemaMove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: "compliance");

            migrationBuilder.RenameTable(
                name: "compliance_records",
                newName: "compliance_records",
                newSchema: "compliance"
            );

            migrationBuilder.RenameTable(
                name: "audit_events",
                newName: "audit_events",
                newSchema: "compliance"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "compliance_records",
                schema: "compliance",
                newName: "compliance_records"
            );

            migrationBuilder.RenameTable(
                name: "audit_events",
                schema: "compliance",
                newName: "audit_events"
            );
        }
    }
}
