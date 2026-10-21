using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class VatDefaultConcurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_vat_schemes_is_default",
                schema: "invoicing",
                table: "vat_schemes"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_vat_schemes_is_default",
                schema: "invoicing",
                table: "vat_schemes",
                column: "is_default",
                unique: true,
                filter: "is_default"
            );
        }
    }
}
