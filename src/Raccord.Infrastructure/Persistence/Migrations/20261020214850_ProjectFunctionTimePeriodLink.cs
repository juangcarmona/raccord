using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ProjectFunctionTimePeriodLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "project_time_period_id",
                schema: "crew",
                table: "project_functions",
                type: "uuid",
                nullable: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_project_functions_project_id_project_time_period_id",
                schema: "crew",
                table: "project_functions",
                columns: new[] { "project_id", "project_time_period_id" }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_project_functions_project_id_project_time_period_id",
                schema: "crew",
                table: "project_functions"
            );

            migrationBuilder.DropColumn(
                name: "project_time_period_id",
                schema: "crew",
                table: "project_functions"
            );
        }
    }
}
