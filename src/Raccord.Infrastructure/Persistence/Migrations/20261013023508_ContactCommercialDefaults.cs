using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ContactCommercialDefaults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "default_discount_percent",
                table: "projects",
                type: "numeric(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m
            );

            migrationBuilder.AddColumn<string>(
                name: "payment_terms",
                table: "projects",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true
            );

            migrationBuilder.AddColumn<decimal>(
                name: "default_discount_percent",
                schema: "contacts",
                table: "customers",
                type: "numeric(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m
            );

            migrationBuilder.AddColumn<string>(
                name: "default_payment_terms",
                schema: "contacts",
                table: "customers",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "default_discount_percent", table: "projects");

            migrationBuilder.DropColumn(name: "payment_terms", table: "projects");

            migrationBuilder.DropColumn(
                name: "default_discount_percent",
                schema: "contacts",
                table: "customers"
            );

            migrationBuilder.DropColumn(
                name: "default_payment_terms",
                schema: "contacts",
                table: "customers"
            );
        }
    }
}
