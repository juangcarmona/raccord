using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Letterheads : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: "documents");

            migrationBuilder.CreateTable(
                name: "letterheads",
                schema: "documents",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(
                        type: "character varying(200)",
                        maxLength: 200,
                        nullable: false
                    ),
                    document_types = table.Column<string>(
                        type: "character varying(1000)",
                        maxLength: 1000,
                        nullable: false
                    ),
                    page_width_millimetres = table.Column<decimal>(
                        type: "numeric(8,2)",
                        precision: 8,
                        scale: 2,
                        nullable: false
                    ),
                    page_height_millimetres = table.Column<decimal>(
                        type: "numeric(8,2)",
                        precision: 8,
                        scale: 2,
                        nullable: false
                    ),
                    margin_top_millimetres = table.Column<decimal>(
                        type: "numeric(8,2)",
                        precision: 8,
                        scale: 2,
                        nullable: false
                    ),
                    margin_right_millimetres = table.Column<decimal>(
                        type: "numeric(8,2)",
                        precision: 8,
                        scale: 2,
                        nullable: false
                    ),
                    margin_bottom_millimetres = table.Column<decimal>(
                        type: "numeric(8,2)",
                        precision: 8,
                        scale: 2,
                        nullable: false
                    ),
                    margin_left_millimetres = table.Column<decimal>(
                        type: "numeric(8,2)",
                        precision: 8,
                        scale: 2,
                        nullable: false
                    ),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    storage_key = table.Column<string>(
                        type: "character varying(300)",
                        maxLength: 300,
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_letterheads", x => x.id);
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_letterheads_name",
                schema: "documents",
                table: "letterheads",
                column: "name",
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "letterheads", schema: "documents");
        }
    }
}
