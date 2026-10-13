using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ComplianceRecordChain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "previous_hash",
                schema: "compliance",
                table: "compliance_records",
                type: "character(64)",
                fixedLength: true,
                maxLength: 64,
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "record_hash",
                schema: "compliance",
                table: "compliance_records",
                type: "character(64)",
                fixedLength: true,
                maxLength: 64,
                nullable: false,
                defaultValue: ""
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "previous_hash",
                schema: "compliance",
                table: "compliance_records"
            );

            migrationBuilder.DropColumn(
                name: "record_hash",
                schema: "compliance",
                table: "compliance_records"
            );
        }
    }
}
