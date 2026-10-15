using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RefundableDeposits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "deposit_status",
                table: "projects",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.AddColumn<decimal>(
                name: "refundable_deposit_amount",
                table: "projects",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "deposit_status", table: "projects");

            migrationBuilder.DropColumn(name: "refundable_deposit_amount", table: "projects");
        }
    }
}
